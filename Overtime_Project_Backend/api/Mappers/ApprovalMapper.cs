using api.Domain;
using api.DTOs;

namespace api.Mappers
{
    public static class ApprovalMapper
    {
        public static ApprovalResponseDto ToDto(this Approval a)
        {
            return new ApprovalResponseDto
            {
                ApprovalId = a.ApprovalId,
                OvertimeId = a.OvertimeId,
                ManagerId = a.ManagerId,
                ApprovedHours = a.ApprovedHours,
                ApprovalDate = a.ApprovalDate,
                Status = a.Status.ToString(),
                Comments = a.Comments,
                RejectionReason = a.RejectionReason
            };
        }
    }
}
