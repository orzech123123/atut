using System.Collections.Generic;
using System.Linq;
using Atut.Models;
using Atut.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Atut.Services
{
    public class VatNumberService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationManager _notificationManager;

        public VatNumberService(
            DatabaseContext databaseContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            INotificationManager notificationManager
            )
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _notificationManager = notificationManager;
        }

        public VatNumbersViewModel GetByUserId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                var loggedUser = _databaseContext.Users.Single(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
                id = loggedUser.Id;
            }

            var vatNumbers = _databaseContext.VatNumbers
                .Include(vat => vat.User)
                .Where(vat => vat.User.Id == id)
                .ToList();

            var vatNumbersViewModels = _mapper.Map<IEnumerable<VatNumber>, IEnumerable<VatNumberViewModel>>(vatNumbers).ToList();
            CountriesHelper.Countries.ToList().ForEach(country =>
            {
                if (vatNumbersViewModels.All(vat => vat.CountryName != country))
                {
                    vatNumbersViewModels.Add(new VatNumberViewModel
                    {
                        CountryName = country
                    });
                }
            });

            var user = _databaseContext.Users.Single(u => u.Id == id);
            var company = new KeyValueViewModel { Key = user.Id, Value = user.CompanyNameShort };

            vatNumbersViewModels = vatNumbersViewModels.OrderBy(vat => vat.CountryName).ToList();

            var viewModel = new VatNumbersViewModel
            {
                VatNumbers = vatNumbersViewModels,
                Company = company
            };

            return viewModel;
        }

        public void Save(VatNumbersViewModel viewModel)
        {
            var numbersToRemove = _databaseContext.VatNumbers
                .Include(vat => vat.User)
                .Where(vat => vat.User.Id == viewModel.Company.Key)
                .ToList();

            _databaseContext.VatNumbers.RemoveRange(numbersToRemove);

            var numbersToAdd = new List<VatNumber>();

            _mapper.Map(viewModel.VatNumbers.Where(vat => !string.IsNullOrWhiteSpace(vat.Number)), numbersToAdd);

            var user = _databaseContext.Users.Single(u => u.Id == viewModel.Company.Key);
            numbersToAdd.ForEach(num => num.User = user);

            _databaseContext.VatNumbers.AddRange(numbersToAdd);

            _notificationManager.Add(NotificationType.Information, "Numery VAT zostały zaktualizowane.");
        }
    }
}
