﻿using Application.GlobalExceptionHandling.Exceptions;
using Application.Interfaces;
using Application.ViewModels.GroupDTOs;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork,IClaimsService claimsService,IMapper mapper)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GroupViewDTO> CreateGroupAsync(GroupCreateDTO createDTO)
        {
            var groupDTO = _mapper.Map<Group>(createDTO);
            groupDTO.TourGuideId=_claimsService.GetCurrentUser;
            var result= await _unitOfWork.GroupRepository.AddAsync(groupDTO);
            var menuCreate = new Menu { GroupId = result.Id, Quantity = 0, TourGuideId = _claimsService.GetCurrentUser };
            await _unitOfWork.MenuRepository.AddAsync(menuCreate);
            if (await _unitOfWork.SaveChangeAsync() > 0)
            {
                var groupView = _mapper.Map<GroupViewDTO>(result);
                groupView.MenuId = menuCreate.Id;
                return groupView;
            }
            throw new BadRequestException("Create fail!");
        }

        public async Task<List<GroupViewDTO>> GetAllGroupAsync()
        {
            var listGroup = (await _unitOfWork.GroupRepository.GetAllAsync(x=>x.Menus)).Where(x=>x.TourGuideId==_claimsService.GetCurrentUser).ToList();
            if (listGroup.Count==0) throw new NotFoundException("No Group!");
            var mapper = _mapper.Map<List<GroupViewDTO>>(listGroup);
            return GetMenuId(mapper,listGroup);
        }

        private List<GroupViewDTO> GetMenuId(List<GroupViewDTO> list,List<Group> groups)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var menu = groups[i].Menus.FirstOrDefault();
                if(menu!= null)
                {
                    list[i].MenuId = menu.Id;
                }
            }
            return list;
        }

        public async Task<GroupViewDTO> GetGroupByIdAsyn(Guid groupId)
        {
            var group= await _unitOfWork.GroupRepository.FindByField(x=>x.Id==groupId&& x.TourGuideId==_claimsService.GetCurrentUser,x=>x.Menus);
            if (group == null) throw new NotFoundException("There is no group " + groupId + " in system!");
            var result= _mapper.Map<GroupViewDTO>(group);
            result.MenuId=group.Menus.First().Id;
            return result;
        }

        public async Task<bool> UpdateGroupAsync(GroupUpdateDTO updateDTO)
        {
            var group = await _unitOfWork.GroupRepository.GetByIdAsync(updateDTO.Id);
            if(group == null) throw new NotFoundException("There is no group " + updateDTO.Id + " in system!");
            var mapper = _mapper.Map(updateDTO, group);
            _unitOfWork.GroupRepository.Update(group);
            var result = await _unitOfWork.SaveChangeAsync();
            return result>0;
        }
    }
}
