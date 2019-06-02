﻿using iChat.Api.Extensions;
using iChat.Api.Models;
using iChat.Api.Services;
using iChat.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iChat.Api.Dtos;
using iChat.Api.Constants;

namespace iChat.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChannelsController : ControllerBase {
        private IChannelService _channelService;
        private ICacheService _cacheService;

        public ChannelsController(iChatContext context,
            IChannelService channelService,
            ICacheService cacheService) {
            _channelService = channelService;
            _cacheService = cacheService;
        }

        // GET api/channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Channel>>> GetAsync() {
            try {
                var channels = await _channelService
                    .GetChannelsAsync(User.GetUserId(), User.GetWorkplaceId());
                return channels.ToList();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET api/channels/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Channel>> GetAsync(int id) {
            try {
                if (id == iChatConstants.DefaultChannelIdInRequest) {
                    id = await _channelService.GetDefaultChannelGeneralIdAsync(User.GetWorkplaceId());
                }
                
                var channel = await _channelService.GetChannelByIdAsync(id, User.GetWorkplaceId());
                if (channel == null) {
                    return NotFound();
                }

                return channel;
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // POST api/channels
        [HttpPost]
        public async Task<ActionResult<int>> CreateChannelAsync(ChannelCreateDto channelCreateDto) {
            try
            {
                var id = await _channelService.CreateChannelAsync(channelCreateDto.Name, User.GetWorkplaceId(), channelCreateDto.Topic);
                await _channelService.AddUserToChannelAsync(id, User.GetUserId(), User.GetWorkplaceId());

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }        
    }
}
