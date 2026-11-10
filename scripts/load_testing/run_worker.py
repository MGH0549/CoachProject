"""One persistent HTTPS connection per load-test worker."""
import http.client
import ssl
import time


def run_worker(settings):
    host, port, base_path, token, iterations, ca_file = settings
    connection = http.client.HTTPSConnection(
        host, port, context=ssl.create_default_context(cafile=ca_file), timeout=15
    )
    samples = []
    paths = ("/api/v1/me/training", "/api/v1/me/progress")
    try:
        for index in range(iterations + 1):
            for path in paths:
                started = time.perf_counter()
                status = 0
                try:
                    connection.request("GET", base_path + path, headers={"Authorization": "Bearer " + token})
                    response = connection.getresponse()
                    status = response.status
                    response.read()
                except (OSError, http.client.HTTPException):
                    connection.close()
                if index > 0:
                    samples.append((path, (time.perf_counter() - started) * 1000, status))
    finally:
        connection.close()
    return samples
