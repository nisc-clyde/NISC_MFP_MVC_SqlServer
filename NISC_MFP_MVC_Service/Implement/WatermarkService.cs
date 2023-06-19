﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Watermark;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class WatermarkService : IWatermarkService
    {
        private readonly IWatermarkRepository _watermarkRepository;
        private readonly Mapper _mapper;

        public WatermarkService()
        {
            _watermarkRepository = new WatermarkRepository();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(WatermarkInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _watermarkRepository.Insert(_mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
        }

        public IQueryable<WatermarkInfo> GetAll()
        {
            IQueryable<InitialWatermarkRepoDTO> dataModel = _watermarkRepository.GetAll();

            return dataModel.ProjectTo<WatermarkInfo>(_mapper.ConfigurationProvider);
        }

        public IQueryable<WatermarkInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _watermarkRepository.GetAll(dataTableRequest).ProjectTo<WatermarkInfo>(_mapper.ConfigurationProvider);
        }

        public WatermarkInfo Get(string column, string value, string operation)
        {
            column = column ?? throw new ArgumentNullException(nameof(column), "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException(nameof(value), "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException(nameof(operation), "operation - Reference to null instance.");

            InitialWatermarkRepoDTO dataModel = null;
            if (operation == "Equals")
            {
                dataModel = _watermarkRepository.Get(column, value, ".ToString().ToUpper() == @0");
            }
            else if (operation == "Contains")
            {
                dataModel = _watermarkRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
            }

            if (dataModel == null)
            {
                return null;
            }
            return _mapper.Map<InitialWatermarkRepoDTO, WatermarkInfo>(dataModel);
        }

        public IQueryable<WatermarkInfo> GetWithGlobalSearch(IQueryable<WatermarkInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<WatermarkInfo> resultModel = searchData
                    .Where(p =>
                     p.type.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.left_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.right_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.top_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.bottom_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.position_mode.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    ((!string.IsNullOrEmpty(p.fill_mode)) && p.fill_mode.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.text)) && p.text.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.image_path)) && p.image_path.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((p.rotation != null) && p.rotation.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.color)) && p.color.ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<WatermarkInfo> GetWithColumnSearch(IQueryable<WatermarkInfo> searchData, string column, string searchValue)
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Delete(WatermarkInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _watermarkRepository.Delete(_mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
        }

        public void Update(WatermarkInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _watermarkRepository.Update(_mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
        }

        public void SaveChanges()
        {
            _watermarkRepository.SaveChanges();
        }

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _watermarkRepository.Dispose();
        }
    }
}
