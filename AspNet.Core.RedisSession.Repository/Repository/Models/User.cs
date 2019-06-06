using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNet.Core.RedisSession.Repository.Model
{
    /// <summary>
    /// 測試用 Table 實體資料
    /// </summary>
    [Table("user")]
    public class User
    {
        /// <summary>
        /// 編號
        /// </summary>
        [Key]
        [Column("user_id")]
        public string Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Column("user_name")]
        public string Name { get; set; }
        
        /// <summary>
        /// 密碼
        /// </summary>
        [Column("user_password")]
        public string Password { get; set; }
    }
}