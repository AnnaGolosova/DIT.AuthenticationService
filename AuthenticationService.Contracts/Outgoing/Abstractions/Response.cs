namespace AuthenticationService.Contracts.Outgoing.Abstractions
{
    public class Response
    {
        /// <summary>
        ///     Gets or sets a value indicating whether or not the action was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///     Gets a <see cref="Response" /> instance indicating success.
        /// </summary>
        public static Response Successfull { get; } = new Response { Success = true };

        /// <summary>
        ///     Gets a <see cref="Response" /> instance indicating an error.
        /// </summary>
        public static Response Error { get; } = new Response { Success = false };
    }
}
