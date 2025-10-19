﻿using Domain.Models;
using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class AttendeeService : IAttendeeService
    {
        private readonly IRepository<Attendee> _Repository;
        public AttendeeService (IRepository<Attendee> repository)
        {
            _Repository = repository;
        }
        public List<Attendee> GetAll()
        {
            return _Repository.GetAll(selector: x => x).ToList();
        }

        public Attendee? GetById(Guid Id)
        {
            return _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
        }
        public Attendee Add(Attendee attendee)
        {
            attendee.Id = Guid.NewGuid();
            return _Repository.Insert(attendee);
        }

        public Attendee DeleteById(Guid Id)
        {
            var attendee = _Repository.Get(selector: x => x, predicate: x => x.Id == Id);
            return _Repository.Delete(attendee);
        }

        public Attendee Update(Attendee attendee)
        {
            return _Repository.Update(attendee);
        }
    }
}
