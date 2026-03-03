using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Documents
{
    /// <summary>
    /// MongoDB document for a rental.
    /// </summary>
    public class RentalDocument
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the user/person identifier.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the fleet vehicle identifier.
        /// </summary>
        public string FleetId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the fleet vehicle was rented.
        /// </summary>
        public DateTime RentedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the fleet vehicle was returned; null if still rented.
        /// </summary>
        public DateTime? ReturnedAt { get; set; }
    }
}
