﻿using AirportSimulator.data.DTO;
using AirportSimulator.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.services.Interfaces
{
    public interface IAirportService
    {
        void AddAirplane(AirplaneDbDto a);

        public void ReportStations(StationDbDto[] stations);
        public void AddVisit(Visit visit);
        public void UpdeateVisit(Airplane a, DateTime ET);
        public List<Tuple<Station?, Station?>> GetRoute(FlightType flightType);

    }
}
