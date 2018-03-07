using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Autyan.NiChiJou.Core.Data
{
    public abstract class BaseEntity<TKey, TUserKey> : BaseEntity, ICreateTrace<TUserKey>, IModifyTrace<TUserKey>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual TKey Id { get; set; }

        public TUserKey CreatedBy { get; set; }

        public TUserKey ModifiedBy { get; set; }
    }

    public class BaseEntity
    {
        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
