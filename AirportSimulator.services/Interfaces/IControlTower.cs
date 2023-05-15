﻿using AirportSimulator.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.services.Interfaces
{
    public interface IControlTower
    {
        public Airplane AddFlight(Airplane a);

        public IEnumerable<Airplane> GetAirplanes();
    }
}
