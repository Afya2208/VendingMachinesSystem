using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DesktopApp.Util;

public class Validator
{
    private readonly StringBuilder _stringBuilder = new();

    public void Validate(bool condition, string message)
    {
        if (condition) 
        {
            _stringBuilder.AppendLine(message);
        }
    }
    public bool GetVerdict()
    {
        if (_stringBuilder.Length != 0)
        {
            ShowError("Ошибка", _stringBuilder.ToString());
        }
        return _stringBuilder.Length == 0;
    }
}