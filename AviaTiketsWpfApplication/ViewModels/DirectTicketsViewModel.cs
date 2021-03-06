﻿using AviaTicketsWpfApplication.Fundamentals.Abstracts;
using AviaTicketsWpfApplication.Fundamentals.Interfaces;
using AviaTicketsWpfApplication.Models;
using AviaTicketsWpfApplication.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using TravelpayoutsAPI.Library.Infostructures.Interfaces;
using TravelpayoutsAPI.Library.Models;
using TravelpayoutsAPI.Library.Models.Data;

namespace AviaTicketsWpfApplication.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DirectTicketsViewModel : BasePageSearchResultViewModel<Ticket>
    {
        private IEnumerable<City> _cities;

        /// <summary>
        /// Initializes a new instance of the DirectTiketsViewModel class.
        /// </summary>
        public DirectTicketsViewModel(ISearchTicketApiFactory searchTicketApiFactory, ICacheService cacheService)
            : base(searchTicketApiFactory, cacheService) 
        {
        }

        protected override async Task InitializeAsync()
        {
            var message = new ViewModelMessage { IsSearhEnabled = true, IsShowedSearhPanel = true, IsShowingProgress = true };

            MessengerInstance.Send(
                    new FlyoutMessage { TypeViewModel = typeof(SearchViewModel), Header = "Search tickets", IsRightPosition = true });
            MessengerInstance.Send(message);

            _cities = await _cacheService.GetAsync<List<City>>(DataNames.Cities);

            await base.InitializeAsync();

            message.IsShowingProgress = false;
            MessengerInstance.Send(message);
        }

        protected override async Task<IEnumerable<Ticket>> UpdateCollection(ISearchQuery searchQuery)
        {
            var query = searchQuery as SearchQuery;
            if (query == null)
            {
                SendError(Resources.ErrorParams);
                return null;
            }

            var token = await _token.Value;
            try
            {
                IEnumerable<Ticket> tickets;
                if (query.Destination != null && query.DepartDate.HasValue && query.ReturnDate.HasValue)
                {
                    tickets = await _searchTicketApiFactory.SimpleSearch
                        .GetDirectFlights(token, query.Original.Code, query.Destination.Code, query.DepartDate, query.ReturnDate);
                }
                else
                {
                    tickets = await _searchTicketApiFactory.SimpleSearch
                        .GetDirectFlights(token, query.Original.Code, query.Destination != null ? query.Destination.Code : null);
                }

                var list = tickets.ToList();
                foreach (var ticket in list)
                {
                    var origin = _cities.SingleOrDefault(c => c.Code.Equals(ticket.Origin));
                    var destination = _cities.SingleOrDefault(c => c.Code.Equals(ticket.Destination));

                    ticket.Origin = origin.CultureName;
                    ticket.Destination = destination.CultureName;
                }

                return list;
            }
            catch (HttpRequestException)
            {
                SendError(Resources.Error404);
            }

            return null;
        }
    }
}