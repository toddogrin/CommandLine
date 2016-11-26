namespace CommandLine {

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CommandLineUsage : System.Attribute {

        public string ExecutableName { get; private set; }

        public string PreUsageMessage { get; private set; }

        public string PostUsageMessage { get; private set; }

        public CommandLineUsage() : this(null, null, null) { }

        public CommandLineUsage(string executableName) : this(executableName, null, null) { }

        public CommandLineUsage(string executableName, string preUsageMessage, string postUsageMessage) {
            this.ExecutableName = executableName;
            this.PreUsageMessage = preUsageMessage;
            this.PostUsageMessage = postUsageMessage;
        }
    }
}

