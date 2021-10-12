using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Commands.Abstractions
{
    public abstract class BaseCommand<TEntity, TResponse> : IRequest<TResponse>
        where TEntity : class
    {
        public TEntity Entity { get; set; }

        protected BaseCommand(TEntity entity)
        {
            Entity = entity;
        }
    }
}
