using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol.Server;

namespace MCPServerWeb.Tools
{
    /// <summary>
    /// Mock tools for demonstrating MCP client data operations
    /// </summary>
    [McpServerToolType]
    public static class MockTools
    {
        private static readonly List<ClientListItem> _mockClients = new()
        {
            new ClientListItem { Id = 1, Name = "Acme Corporation", Status = ClientStatus.Active },
            new ClientListItem { Id = 2, Name = "TechStart Industries", Status = ClientStatus.Active },
            new ClientListItem { Id = 3, Name = "Global Solutions Ltd", Status = ClientStatus.Inactive },
            new ClientListItem { Id = 4, Name = "Innovation Labs", Status = ClientStatus.Pending },
            new ClientListItem { Id = 5, Name = "Enterprise Systems Inc", Status = ClientStatus.Active }
        };

        private static readonly Dictionary<int, ClientDetails> _mockClientDetails = new()
        {
            {
                1, new ClientDetails
                {
                    Id = 1,
                    Name = "Acme Corporation",
                    Status = ClientStatus.Active,
                    Email = "contact@acmecorp.com",
                    Phone = "+1-555-0101",
                    Address = "123 Business Ave, Suite 100, New York, NY 10001",
                    AccountManager = "Sarah Johnson",
                    AnnualRevenue = 2500000.00m,
                    JoinDate = new DateTime(2020, 3, 15),
                    LastContactDate = new DateTime(2025, 10, 28),
                    Industry = "Manufacturing",
                    EmployeeCount = 150
                }
            },
            {
                2, new ClientDetails
                {
                    Id = 2,
                    Name = "TechStart Industries",
                    Status = ClientStatus.Active,
                    Email = "info@techstart.io",
                    Phone = "+1-555-0202",
                    Address = "456 Innovation Drive, San Francisco, CA 94105",
                    AccountManager = "Michael Chen",
                    AnnualRevenue = 1800000.00m,
                    JoinDate = new DateTime(2021, 7, 22),
                    LastContactDate = new DateTime(2025, 11, 01),
                    Industry = "Technology",
                    EmployeeCount = 75
                }
            },
            {
                3, new ClientDetails
                {
                    Id = 3,
                    Name = "Global Solutions Ltd",
                    Status = ClientStatus.Inactive,
                    Email = "admin@globalsolutions.co.uk",
                    Phone = "+44-20-7123-4567",
                    Address = "789 Enterprise Road, London, EC1A 1BB, UK",
                    AccountManager = "Emma Williams",
                    AnnualRevenue = 950000.00m,
                    JoinDate = new DateTime(2019, 11, 10),
                    LastContactDate = new DateTime(2024, 12, 15),
                    Industry = "Consulting",
                    EmployeeCount = 45
                }
            },
            {
                4, new ClientDetails
                {
                    Id = 4,
                    Name = "Innovation Labs",
                    Status = ClientStatus.Pending,
                    Email = "hello@innovationlabs.com",
                    Phone = "+1-555-0404",
                    Address = "321 Research Blvd, Austin, TX 78701",
                    AccountManager = "David Rodriguez",
                    AnnualRevenue = 0.00m,
                    JoinDate = new DateTime(2025, 10, 30),
                    LastContactDate = new DateTime(2025, 10, 30),
                    Industry = "Research & Development",
                    EmployeeCount = 20
                }
            },
            {
                5, new ClientDetails
                {
                    Id = 5,
                    Name = "Enterprise Systems Inc",
                    Status = ClientStatus.Active,
                    Email = "support@entsystems.com",
                    Phone = "+1-555-0505",
                    Address = "654 Corporate Center, Chicago, IL 60601",
                    AccountManager = "Jennifer Martinez",
                    AnnualRevenue = 4200000.00m,
                    JoinDate = new DateTime(2018, 5, 8),
                    LastContactDate = new DateTime(2025, 11, 02),
                    Industry = "Software",
                    EmployeeCount = 280
                }
            }
        };

        [McpServerTool,
            Description("Get a list of all Galaxy clients with their basic information (id, name, status)")]
            //Description("Get a list of all clients with their basic information (id, name, status)")]
        public static string GetClientsList()
        {
            var options = new JsonSerializerOptions
            {
                //WriteIndented = true,
                WriteIndented = false,
                Converters = { new JsonStringEnumConverter() }
            };

            return JsonSerializer.Serialize(_mockClients, options);
        }

        [McpServerTool,
            Description("Get detailed information about a specific Galaxy client by their ID")]
            //Description("Get detailed information about a specific client by their ID")]
        public static string GetClientDetails(int clientId)
        {
            if (_mockClientDetails.TryGetValue(clientId, out var clientDetails))
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                return JsonSerializer.Serialize(clientDetails, options);
            }

            return JsonSerializer.Serialize(new
            {
                error = "Client not found",
                message = $"No client exists with ID {clientId}",
                availableIds = _mockClientDetails.Keys.ToList()
            }, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    /// <summary>
    /// Lightweight client information for list view
    /// </summary>
    public class ClientListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ClientStatus Status { get; set; }
    }

    /// <summary>
    /// Comprehensive client information with all details
    /// </summary>
    public class ClientDetails
    {
        // Basic Information
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ClientStatus Status { get; set; }

        // Contact Information
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Business Information
        public string AccountManager { get; set; } = string.Empty;
        public decimal AnnualRevenue { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastContactDate { get; set; }
        public string Industry { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }

    /// <summary>
    /// Client status enumeration
    /// </summary>
    public enum ClientStatus
    {
        Active,
        Inactive,
        Pending
    }
}
