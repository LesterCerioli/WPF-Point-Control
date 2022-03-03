using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
    {
        public virtual TId Id { get; protected set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Entity<TId>;
            if (entity != null)
            {
                return this.Equals(entity);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        protected virtual void checkPreConstructor() {
            var stackTrace = new System.Diagnostics.StackTrace();
            string invoker = stackTrace.GetFrame(2).GetMethod().DeclaringType.Name;
            Check.Require(this.GetType().Name.Equals(invoker) || invoker.Contains("Factory"), "O construtor só deve ser invocado por uma factory ou para clonagem. Invocado por: " + invoker);
        }

        #region IEquatable<Entity> Members

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Id.Equals(default(TId)))
            {
                //Os objetos ainda não existem no banco de dados
                //Sendo assim, os objetos podem ser diferentes, então vou verificar se é a mesma instância
                return this == other;
            }

            return this.Id.Equals(other.Id);
        }

        #endregion
    }
}
