﻿using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    [ApiController]
    [Route("flags")]
    public class FlagsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices] IFlagAppService flagAppService, [FromBody] Flag flag)
        {
            await flagAppService.CreateAsync(flag);
            return Created($"{Request.Path}", flag);
        }

        
        [HttpGet("{projectId}/{environmentId}/{flagId}")]
        public async Task<IActionResult> GetByEnviromentAsync([FromServices] IFlagAppService flagAppService,
             string projectId, string environmentId, string flagId)
        {
            return Ok(await flagAppService.GetAsync(projectId, environmentId, flagId));
        }

        [HttpGet("{projectId}/{environmentId}/{flagId}/status")]
        public async Task<IActionResult> GetFeatureFlagStatusAsync([FromServices] IFlagAppService flagAppService,
          string projectId, string environmentId, string flagId)
        {
            return Ok(await flagAppService.GetStatusAsync(projectId, environmentId, flagId));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromServices] IFlagAppService flagAppService, [FromBody] Flag flag)
        {
            await flagAppService.UpdateAsync(flag);
            return Ok();
        }        
    }
}
