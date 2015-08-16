﻿//*********************************************************
//
// Copyright (c) 2015 nikitadev. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using Newtonsoft.Json;
using TravelpayoutsAPI.Library.Infostructures.Implements;
using TravelpayoutsAPI.Library.Infostructures.Interfaces;
using TravelpayoutsAPI.Library.Models.Monitor;

namespace TravelpayoutsAPI.Library
{
    public class SearchTicketApiFactory : ISearchTicketApiFactory
    {
        private readonly IRequestManager _requestManager;

        private readonly Func<IUserInfoProvider> _creatorUserInfoProvider;
        private readonly Func<IDataInfoProvider> _creatorDataInfoProvider;
        private readonly Func<IPopularRoutesProvider> _creatorPopularRoutesProvider;
        private readonly Func<ISearchTicketsProvider> _creatorSearchTicketsProvider;
        private readonly Func<ISimpleSearchTicketsProvider> _creatorSimpleSearchTicketsProvider;

        private readonly Func<IFlightSearchProvider> _creatorFlightSearchProvider;

        public string Token { get; private set; }

        public int Marker { get; set; }

        public CurrencyType Currency { get; set; }

        private IUserInfoProvider _userInfoProvider;
        public IUserInfoProvider UserInfo
        {
            get
            {
                if (_userInfoProvider == null)
                {
                    _userInfoProvider = _creatorUserInfoProvider.Invoke();
                }

                return _userInfoProvider;
            }
        }

        private IDataInfoProvider _dataInfoProvider;
        public IDataInfoProvider DataInfo
        {
            get
            {
                if (_dataInfoProvider == null)
                {
                    _dataInfoProvider = _creatorDataInfoProvider.Invoke();
                }

                return _dataInfoProvider;
            }
        }

        private IPopularRoutesProvider _popularRoutesProvider;
        public IPopularRoutesProvider PopularRoutes
        {
            get
            {
                if (_popularRoutesProvider == null)
                {
                    _popularRoutesProvider = _creatorPopularRoutesProvider.Invoke();
                }

                return _popularRoutesProvider;
            }
        }

        private ISearchTicketsProvider _searchTicketsProvider;
        public ISearchTicketsProvider MainSearch
        {
            get
            {
                if (_searchTicketsProvider == null)
                {
                    _searchTicketsProvider = _creatorSearchTicketsProvider.Invoke();
                }

                return _searchTicketsProvider;
            }
        }

        private ISimpleSearchTicketsProvider _simpleSearchTicketsProvider;
        public ISimpleSearchTicketsProvider SimpleSearch
        {
            get
            {
                if (_simpleSearchTicketsProvider == null)
                {
                    _simpleSearchTicketsProvider = _creatorSimpleSearchTicketsProvider.Invoke();
                }

                return _simpleSearchTicketsProvider;
            }
        }

        private IFlightSearchProvider _flightSearchProvider;
        public IFlightSearchProvider FlightSearch
        {
            get
            {
                if (_flightSearchProvider == null)
                {
                    _flightSearchProvider = _creatorFlightSearchProvider.Invoke();
                }

                return _flightSearchProvider;
            }
        }

        public SearchTicketApiFactory()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-dd",
                ContractResolver = new LowercaseContractResolver()
            };

            _requestManager = new RequestManager();

            _creatorUserInfoProvider = new Func<IUserInfoProvider>(() => new UserInfoProvider(_requestManager));
            _creatorDataInfoProvider = new Func<IDataInfoProvider>(() => new DataInfoProvider(_requestManager));
            _creatorPopularRoutesProvider = new Func<IPopularRoutesProvider>(() => new PopularRoutesProvider(_requestManager));
            _creatorSearchTicketsProvider = new Func<ISearchTicketsProvider>(() => new SearchTicketsProvider(_requestManager));
            _creatorSimpleSearchTicketsProvider = new Func<ISimpleSearchTicketsProvider>(() => new SimpleSearchTicketsProvider(_requestManager));

            _creatorFlightSearchProvider = new Func<IFlightSearchProvider>(() => new FlightSearchProvider(_requestManager));
        }
    }
}