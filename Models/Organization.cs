public class Organization
{
    public int OrganizationId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyImage { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    public virtual ICollection<Job> Jobs { get; set; }
}