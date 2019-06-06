using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.Core.RedisSession.Repository.Model
{
    /// <summary>
    /// user entity
    /// </summary>
    [Table("user")]
    public class User
    {
        /// <summary>
        /// user id
        /// </summary>
        [Key]
        [Column("user_id")]
        public string Id { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        [Column("user_name")]
        public string Name { get; set; }
        
        /// <summary>
        /// user password
        /// </summary>
        [Column("user_password")]
        public string Password { get; set; }
    }
}