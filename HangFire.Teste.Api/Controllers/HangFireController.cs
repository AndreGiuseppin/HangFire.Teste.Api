using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Teste.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public HangFireController(IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        [HttpGet]
        public IActionResult EnqueueJob()
        {
            _backgroundJobClient.Enqueue(() => Console.WriteLine("Job Enqueue"));
            return Ok();
        }

        [HttpGet]
        [Route("ScheduleJob")]
        public IActionResult ScheduleJob()
        {
            var scheduleDate = new DateTime(2022, 09, 13, 10, 13, 00);
            _backgroundJobClient.Schedule(() => Console.WriteLine($"Scheduled Job at {scheduleDate}"), scheduleDate);
            return Ok();
        }

        [HttpGet]
        [Route("RecurringJob")]
        public IActionResult RecurringJob()
        {
            var scheduleDate = new DateTime(2022, 09, 13, 10, 00, 00);
            _recurringJobManager.AddOrUpdate("Recurring Job", () => Console.WriteLine("Recurring Job at 15 seconds"), "0/15 * * * * *", TimeZoneInfo.Local);
            return Ok();
        }

        [HttpGet]
        [Route("ContinueJob")]
        public IActionResult ContinueJob()
        {
            var jobId = _backgroundJobClient.Enqueue(() => Console.WriteLine("Continue Job"));
            _backgroundJobClient.ContinueJobWith(jobId, () => Console.WriteLine($"Continue Job with ID {jobId}"));
            return Ok();
        }
    }
}