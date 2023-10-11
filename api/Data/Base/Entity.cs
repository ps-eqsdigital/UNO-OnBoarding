using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Base
{
    public abstract class Entity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }

        public Guid Uuid { get; set; }

        public virtual bool IsDeleted => DeletedAt != default(DateTime);

        public void Update()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        protected Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
