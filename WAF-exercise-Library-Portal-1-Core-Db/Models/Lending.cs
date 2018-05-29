using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WAF_exercise_Library_Portal_1_Core_Db.Models
{
    public enum LendingState
    {
        TOO_SOON_TO_PICK_UP,
        PICKED_UP,
        READY_TO_PICK_UP,
        NOT_PICKED_UP,
        RETURNED,
        NOT_RETURNED,
        ERROR
    }
    public partial class Lending
    {
        [Key]
        public Int32 Id { get; set; }

        [DisplayName("Volume")]
        public String VolumeId { get; set; }

        [DisplayName("ApplicationUser")]
        public Int32 ApplicationUserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public Byte Active { get; set; }

        public virtual Volume Volume { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public LendingState GetState()
        {
            DateTime now = DateTime.UtcNow;

            if (now < StartDate)
            {
                if (Active == 0)
                {
                    return LendingState.TOO_SOON_TO_PICK_UP;
                }
                else
                {
                    return LendingState.ERROR;
                }
            }

            if (now < EndDate)
            {
                if (Active == 0)
                {
                    return LendingState.READY_TO_PICK_UP;
                }
                else if (Active == 1)
                {
                    return LendingState.PICKED_UP;
                }
                else
                {
                    return LendingState.RETURNED;
                }
            }

            if (Active == 0)
            {
                return LendingState.NOT_PICKED_UP;
            }
            else if (Active == 1)
            {
                return LendingState.NOT_RETURNED;
            }
            else
            {
                return LendingState.RETURNED;
            }
        }

        public Boolean IsReturnedOrTheyHaveItLending()
        {
            LendingState lendingState = GetState();

            if (lendingState == LendingState.RETURNED
             || lendingState == LendingState.PICKED_UP
             || lendingState == LendingState.NOT_RETURNED)
            {
                return true;
            }

            return false;
        }

        public Boolean IsApplicationUserActiveLanding(String applicationUserName)
        {
            return ApplicationUser.UserName == applicationUserName && Active == 0;
        }
    }
}
