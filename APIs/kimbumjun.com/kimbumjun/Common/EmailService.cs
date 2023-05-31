using System.Net.Mail;
using System.Text;

namespace kimbumjun.Common;

public class EmailService : IDisposable
{
    private readonly SmtpClient _client;
    public StringBuilder _body;

    private bool disposed = false;

    public EmailService()
    {
        _client = new SmtpClient();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _client.Dispose();
            }

            disposed = true;
        }
    }
}
