using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearnExample.Util;
using CoreLearnExample.Filter;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections;
using Microsoft.Extensions.Caching.Memory;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreLearnExample.API
{
    [Route("api/[controller]")]
    public class JsonpCrosController : Controller
    {
        // GET: api/<controller>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}


       // [ServiceFilter(typeof(JsonpResultFilter))]
        //[Route("api/[controller]/GetValue")]
        [HttpGet("GetValue")]
        [JsonpResultFilter]
        //[HttpGet]
        public string GetValue(string isCallback)
        {

            var str = "";

            return "这是GetVlue返回的值";

        }


        [HttpGet]
        [HttpPost]
       // [JsonpResultFilter]
        public ActionResult UnloadSingle()
        {
            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                Hashtable hash = new Hashtable();
                IFormFileCollection cols = Request.Form.Files;
                if (cols == null || cols.Count == 0)
                {
                    return Json(new { status = -1, message = "没有上传文件", data = hash });
                }
                foreach (IFormFile file in cols)
                {
                    //定义图片数组后缀格式
                    string[] LimitPictureType = { ".JPG", ".JPEG", ".GIF", ".PNG", ".BMP" };
                    //获取图片后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (LimitPictureType.Contains(currentPictureExtension))
                    {

                        //为了查看图片就不在重新生成文件名称了
                        // var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;
                        var new_path = Path.Combine("uploads/images/", file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", new_path);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            //图片路径保存到数据库里面去
                            //再把文件保存的文件夹中
                            file.CopyTo(stream);
                            hash.Add("file", "/" + new_path);

                        }
                    }
                    else
                    {
                        return Json(new { status = -2, message = "请上传指定格式的图片", data = hash });
                    }
                }

                return Json(new { status = 0, message = "上传成功", data = hash });
            }
            catch (Exception ex)
            {

                return Json(new { status = -3, message = "上传失败", data = ex.Message });

            }
        }


        [JsonpResultFilter]
        [HttpGet("getNub")]
        public List<string> GetNnb([FromServices] IMemoryCache _memoryCache)
        {

            //随机产生4位数字
            var list = new List<string>();
            Random random = new Random();
            for (int i = 0; i < 4; i++) {
                int nub = random.Next(0,9);
                list.Add(nub.ToString());
            }
            var token = Guid.NewGuid().ToString();
            var vCode = string.Join("", list);
            list.Add(token);
            var cacheKey = string.Format("vcode_cache_{0}", token);
            _memoryCache.Set(cacheKey, vCode.ToString(), new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1)));
            CookieOptions options = new CookieOptions();
            return list;

        }

        [HttpGet("getNubV")]
        [JsonpResultFilter]
        public bool GetNnbV([FromServices] IMemoryCache _memoryCache,[FromQuery]string code,[FromQuery]string token)
        {

            var cacheKey = string.Format("vcode_cache_{0}", token.ToString());
            var vCode = "";
            if (!_memoryCache.TryGetValue(cacheKey, out vCode)) return false;
            if (vCode.ToLower() != code.ToLower()) return false;
            _memoryCache.Remove(cacheKey);
            return true;

        }

    }
}
