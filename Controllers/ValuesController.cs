using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer;

namespace AsanaWebhookAPITesting.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _config;

        public ValuesController(IConfiguration configuration)
        {
            this._config = configuration;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        //[HttpPost]        
        //public void Post([FromBody] string value)
        //{
        //    Microsoft.Extensions.Primitives.StringValues data;
        //    if (this.HttpContext.Request.Headers.TryGetValue("X-Hook-Secret", out data))
        //    {
        //        this.Response.Headers.Add("X-Hook-Secret", data);
        //    }

        //    if(string.IsNullOrWhiteSpace(value))
        //    {
        //        return;
        //    }
        //    using (SqlConnection sqlcon=new SqlConnection(_config["DefaultConnection"]))
        //    {
        //        sqlcon.Open();
        //        using(SqlCommand cmd = new SqlCommand("INSERT INTO dbo.ASANA_WebhookAPI([Value],[Time]) VALUES(N'" + value+ "',GETDATE()) ", sqlcon))
        //        {
        //            cmd.ExecuteNonQuery();
        //        }
                

        //    }
        //}

        [HttpPost]
        public  void Post()
        {
            Microsoft.Extensions.Primitives.StringValues data;
            if (this.HttpContext.Request.Headers.TryGetValue("X-Hook-Secret", out data))
            {
                this.Response.Headers.Add("X-Hook-Secret", data);
            }
            var bodyStr = "";
            var req = HttpContext.Request;

            // Allows using several time the stream in ASP.Net Core
            

            // Arguments: Stream, Encoding, detect encoding, buffer size 
            // AND, the most important: keep stream opened
            using (StreamReader reader
                      = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEnd();
            }

            // Rewind, so the core is not lost when it looks the body for the request
            //req.Body.Position = 0;
            if (string.IsNullOrWhiteSpace(bodyStr))
            {
                return;
            }
            using (SqlConnection sqlcon = new SqlConnection(_config["DefaultConnection"]))
            {
                sqlcon.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO dbo.ASANA_WebhookAPI([Value],[Time]) VALUES(N'" + bodyStr + "',GETDATE()) ", sqlcon))
                {
                    cmd.ExecuteNonQuery();
                }


            }
        }


        //public async Task<string> PostRawBufferManual()
        //{

        //string result = await Request.Content.ReadAsStringAsync();
        //return result;
        //}

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
