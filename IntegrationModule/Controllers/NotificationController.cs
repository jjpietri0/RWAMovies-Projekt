using IntegrationModule.Models;
using IntegrationModule.Properties;
using IntegrationModule.REQModels;
using IntegrationModule.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly ProjectDBContext _context;
        private readonly SmtpClientSettings smtpClient;

        public NotificationController(ProjectDBContext dbContext, IOptions<SmtpClientSettings> smtpOptions)
        {
            _context = dbContext;
            smtpClient = smtpOptions.Value;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<NotificationResponse>> GetAll()
        {
            try
            {
                var allNotifications =
                    _context.Notification.Select(dbNotification =>
                    new NotificationResponse
                    {
                        Id = dbNotification.Id,
                        ReceiverEmail = dbNotification.ReceiverEmail,
                        SentAt = dbNotification.SentAt,
                        Subject = dbNotification.Subject,
                        Body = dbNotification.Body
                    });
                return Ok(allNotifications);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<NotificationResponse> GetById(int id)
        {
            try
            {
                var dbNotification = _context.Notification.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    SentAt = dbNotification.SentAt,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<NotificationResponse> Create(NotificationReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = new Notification
                {
                    ReceiverEmail = request.ReceiverEmail,
                    Subject = request.Subject,
                    Body = request.Body,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Notification.Add(dbNotification);

                _context.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    SentAt = dbNotification.SentAt,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("[action]/{id}")]
        public ActionResult<NotificationResponse> Update(int id, [FromBody] NotificationReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = _context.Notification.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                dbNotification.ReceiverEmail = request.ReceiverEmail;
                dbNotification.Subject = request.Subject;
                dbNotification.Body = request.Body;
                dbNotification.SentAt = DateTime.UtcNow;

                _context.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    SentAt = dbNotification.SentAt,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult<NotificationResponse> Remove(int id)
        {
            try
            {
                var dbNotification = _context.Notification.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                _context.Notification.Remove(dbNotification);

                _context.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    SentAt = dbNotification.SentAt,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public ActionResult SendAllNotifications()
        {
            var client = new SmtpClient(smtpClient.Host, smtpClient.Port);
            var sender = smtpClient.SenderEmail;

            try
            {
                var unsentNotifications =
                    _context.Notification.Where(
                        x => !x.SentAt.HasValue);

                foreach (var notification in unsentNotifications)
                {
                    try
                    {
                        var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(notification.ReceiverEmail))
                        {
                            Subject = notification.Subject,
                            Body = notification.Body
                        };

                        client.Send(mail);

                        notification.SentAt = DateTime.UtcNow;
                        _context.SaveChanges();

                    }
                    catch (Exception)
                    {
                        // Nothing to do here
                    }
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<int> GetUnsentNotificationsCount()
        {
            try
            {
                var unsentCount = _context.Notification.Count(x => x.SentAt == null);
                return Ok(unsentCount);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
