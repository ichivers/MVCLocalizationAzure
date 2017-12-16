using AutoMapper;
using Azure_Localization.Api.v1.Models;
using AzureResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Azure_Localization.Api.v1.Controllers
{
    [RoutePrefix("api/v1/localization")]
    public class LocalizationController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> getResourceClasses()
        {
            Mapper.CreateMap<string, LocalizationResourceClass>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom<string>(src => src.Replace(".", "-")));
            IList<string> source = await Service.GetResourceClasses();
            var model = Mapper.Map<IList<string>, List<LocalizationResourceClass>>(source);
            return Ok(model);
        }

        [HttpGet]
        [Route("{area}")]
        public async Task<IHttpActionResult> getResources(string area)
        {
            Mapper.CreateMap<Entity, Localization>()
                .ConvertUsing(e => new Localization()
                {
                    Area = string.Format("{0}", e.PartitionKey.Replace(".", "-")),
                    Value = e.Value,
                    Culture = e.Culture,
                    Key = string.Format("{0}", e.RowKey.Replace(".", "-"))
                });
            IList<Entity> source = await Service.GetResources(area.Replace("-", "."));
            var model = Mapper.Map<IList<Entity>, List<Localization>>(source);
            return Ok(model);
        }

        [HttpPost]
        [Route("{area}/{key}")]
        public async Task<IHttpActionResult> postResource(Localization resource)
        {
            Mapper.CreateMap<Localization, Entity>()
                .ConvertUsing(e => new Entity()
                {
                    PartitionKey = string.Format("{0}", e.Area.Replace("-", ".")),
                    RowKey = string.Format("{0}{1}", e.Key.Substring(0, e.Key.LastIndexOf("-")).Replace("-", "."), e.Key.Substring(e.Key.LastIndexOf("-"))),
                    Value = e.Value,
                    Culture = e.Culture
                });
            var model = Mapper.Map<Localization, Entity>(resource);
            await Task.Run(() => { Service.AddResource(model); });
            return Ok();
        }
    }
}
