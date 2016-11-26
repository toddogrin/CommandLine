using System;

namespace CommandLine {

    /**
     * An attribute that can be applied to module-level local variables in a derivative
     * of AbstractCommandLine. Doing so will bind the attributed field to a particular
     * command line option (and optional value), as defined by the properties of this
     * attribute.
     */
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CommandLineOption : System.Attribute, IComparable<CommandLineOption> {

        public const char TARGET_SHORT_SYMBOL = ' ';

        /**
         * The short symbol of command line option. For example, in Unix you might type 
         * 
         * rm -f BadFile.txt
         * 
         * The -f is the short symbol.
         */
        public char ShortSymbol { get; private set; }

        /**
         * The long symbol of command line option. For example, in Unix you might type 
         * 
         * rm -force BadFile.txt
         * 
         * The -force is the long symbol.
         */
        public string LongSymbol { get; private set; }

        /**
         * If this field is required, this should be set to true. If false, the field is 
         * optional.
         */
        public bool IsRequired { get; private set; }

        /**
         * The text description of the command line option. This will appear in the usage.
         */
        public string Description { get; private set; }

        /**
         * Returns true if this is a target option (i.e. not a flag, but rather the flagless target argument, 
         * as in the "/tmp/*" in "rm -f /tmp/*").
         */
        public bool IsTargetOption { get { return this.ShortSymbol == TARGET_SHORT_SYMBOL; } }

        /**
         * Optional values (i.e. not IsRequired) can be given default values through this
         * property. If optional but no default is specified, the C# default() will be
         * assigned when the option is missing. If required, this default value is ignored.
         */
        public string DefaultValue { get; private set; }

        /**
         * Creates a fully specified flagged command line option.
         */
        public CommandLineOption(string description, bool isRequired, char shortSymbol, string longSymbol, string defaultValue) {
            this.ShortSymbol = shortSymbol;
            this.LongSymbol = longSymbol;
            this.IsRequired = isRequired;
            this.Description = description;
            this.DefaultValue = defaultValue;
        }

        /**
         * Creates a flagged command line option with no default value.
         */
        public CommandLineOption(string description, bool isRequired, char shortSymbol, string longSymbol) 
        : this(description, isRequired, shortSymbol, longSymbol, null) 
        { }

        /**
         * Creates a target command line option.
         */
        public CommandLineOption(string description, bool isRequired, string defaultValue)
        : this(description, isRequired, TARGET_SHORT_SYMBOL, null, defaultValue) 
        { }

        /**
         * Creates a target command line option with no default value.
         */
        public CommandLineOption(string description, bool isRequired)
        : this(description, isRequired, TARGET_SHORT_SYMBOL, null, null) 
        { }

        /**
         * Compares one command line option against another.
         */
        public int CompareTo(CommandLineOption other) {
            return this.ShortSymbol.CompareTo(other.ShortSymbol);
        }
    }
}

