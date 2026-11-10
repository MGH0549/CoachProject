
namespace Coaching.Application;

public partial interface IStore
{
    void Audit(string action, string recordId);
}
