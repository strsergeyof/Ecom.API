using Ecom.API.Contracts;
using Ecom.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Ecom.API.Controllers
{
    public class ProjectController(IDataRepository dataRepository) : Controller
    {


        /// <summary>
        /// Загрузка нового магазина
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost("/project")]
        //public async Task<IActionResult> AddStoreAsync([FromQuery] int id)
        //{
        //    await dataRepository.LoadStore(id);
        //    return Ok();
        //}

        /// <summary>
        /// Товары, цены и скидки для них. Максимум 1 000 товаров. Цена и скидка не могут быть пустыми одновременно.
        /// Если новая цена со скидкой будет хотя бы в 3 раза меньше старой, она попадёт в карантин и товар будет продаваться по старой цене.Ошибка об этом будет в ответах методов Состояния загрузок
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost("/upload")]
        public async Task<IActionResult> Upload([FromBody] UploadPriceDisocuntRequest upload)
        {
            string apiUrl = "https://discounts-prices-api.wildberries.ru/api/v2/upload/task";

            var source = new
            {
                data = upload.PriceDiscounds.Select(x => new
                {
                    nmID = x.NmId,
                    price = x.Price,
                    discount = x.Discount
                }).ToArray()
            };

            var js = System.Text.Json.JsonSerializer.Serialize(source);
            var jsonContent = new StringContent(js, Encoding.UTF8, "application/json");


            using (HttpClient httpClient = new HttpClient())
            {
                var hdr = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                hdr.Headers.Add("contentType", "application/json");
                hdr.Content = jsonContent;
                hdr.Headers.Add("Authorization", upload.Token);

                HttpResponseMessage response = await httpClient.SendAsync(hdr);

                if (response.IsSuccessStatusCode)
                {
                    int statusCode = (int)response.StatusCode;
                    return Ok(new { status = statusCode });
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { status = (int)response.StatusCode, error = errorMessage });
                }
            }

        }
    }
}