﻿using SuggeBook.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggeBook.Domain.Model
{
    public abstract class BaseModel
    {
        public string Id { get; set; }

        public abstract bool TestValidation();

        protected string WrongProperties { get; set; }

        protected bool TestWrongProperties ()
        {
            if (!string.IsNullOrEmpty(WrongProperties))
            {
                throw new InvalidObjectException(this.GetType().ToString(), this.Id, WrongProperties);
            }
            return true;
        }
    }
}
