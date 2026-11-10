"""Run against staging with an active client that has a representative plan and progress history."""
import argparse
import concurrent.futures
import json
import math
import os
from pathlib import Path
import statistics
from urllib.parse import urlparse

from load_testing.run_worker import run_worker

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument("base_url")
parser.add_argument("--workers", type=int, default=5)
parser.add_argument("--iterations", type=int, default=20)
parser.add_argument("--p95-ms", type=float, default=200)
parser.add_argument("--ca-file")
args = parser.parse_args()
url = urlparse(args.base_url)
token = os.environ.get("LOAD_TEST_ACCESS_TOKEN", "")
if url.scheme != "https" or not url.hostname or url.username or url.password or url.query or url.fragment:
    parser.error("Supply an HTTPS base URL without credentials, query, or fragment.")
if not token:
    parser.error("Set LOAD_TEST_ACCESS_TOKEN to an active test client's access token.")
if not 1 <= args.workers <= 20 or not 1 <= args.iterations <= 100:
    parser.error("Use 1-20 workers and 1-100 iterations.")
if args.workers * (args.iterations + 1) * 2 > 280:
    parser.error("Reduce the run to at most 280 requests so the global rate limit does not distort results.")
settings = (url.hostname, url.port or 443, url.path.rstrip("/"), token, args.iterations, args.ca_file)
with concurrent.futures.ThreadPoolExecutor(max_workers=args.workers) as executor:
    batches = list(executor.map(run_worker, [settings] * args.workers))
samples = [sample for batch in batches for sample in batch]
report = {}
for path in ("/api/v1/me/training", "/api/v1/me/progress"):
    rows = [sample for sample in samples if sample[0] == path]
    times = sorted(row[1] for row in rows)
    report[path] = {"requests": len(rows), "failures": sum(row[2] != 200 for row in rows),
                    "median_ms": round(statistics.median(times), 2),
                    "p95_ms": round(times[math.ceil(len(times) * 0.95) - 1], 2)}
output = Path(__file__).resolve().parent.parent / "artifacts" / "load-test-summary.json"
output.parent.mkdir(parents=True, exist_ok=True)
output.write_text(json.dumps(report, indent=2) + "\n", encoding="utf-8")
print(json.dumps(report, indent=2))
raise SystemExit(1 if any(row["failures"] or row["p95_ms"] > args.p95_ms for row in report.values()) else 0)
