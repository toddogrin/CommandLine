using NUnit.Framework;
using System;
using System.Reflection;
using CommandLine;
using System.IO;

namespace CommandLineTest {


	[TestFixture()]
    public class CommandLineTest {

        /**
         * General sample CommandLine class.
         */
        public class SampleCommandLine : AbstractCommandLine {

            [CommandLineOption("string value", false, 's', "string", "whatever")]
            public string StringValue { get; protected set; }

            [CommandLineOption("int value", false, 'i', "int", "123")]
            public int IntValue { get; protected set; }

            [CommandLineOption("bool value", false, 'b', "bool")]
            public bool BoolValue { get; protected set; }

            public SampleCommandLine(String[] args) : base(args) { }

            public SampleCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }

        }

        /**
         * Verify default values are set when no optional values are specified.
         */
        [Test()]
        public void TestCL_SampleDefaults() {
            SampleCommandLine sample = new SampleCommandLine(
                new String[] {}
            );
            Assert.AreEqual(123, sample.IntValue);
            Assert.AreEqual("whatever", sample.StringValue);
            Assert.AreEqual(false, sample.BoolValue);
        }

        /**
         * Verify a single int can be set properly, while other optional values take defaults.
         */
        [Test()]
        public void TestCL_SampleSimple() {
            SampleCommandLine sample = new SampleCommandLine(
                new String[] { "-i", "1234" }
            );
            Assert.AreEqual(1234, sample.IntValue);
            Assert.AreEqual("whatever", sample.StringValue);
            Assert.AreEqual(false, sample.BoolValue);
        }

        /**
         * Verify all optional values are properly set when all values are provided.
         */
        [Test()]
        public void TestCL_SampleMultiple() {
            SampleCommandLine sample = new SampleCommandLine(
                new String[] { "-i", "1234", "-s", "STRING", "-b", "TARGET" }
            );
            Assert.AreEqual(1234, sample.IntValue);
            Assert.AreEqual("STRING", sample.StringValue);
            Assert.AreEqual(true, sample.BoolValue);
        }

        /**
         * Verify the usage printing flag is set properly when -? is on the command line.
         */
        [Test()]
        public void TestCL_SamplePrintUsage() {
            SampleCommandLine sample = new SampleCommandLine(
                new String[] { "-?" }
            );
            Assert.AreEqual(123, sample.IntValue);
            Assert.AreEqual("whatever", sample.StringValue);
            Assert.AreEqual(false, sample.BoolValue);
            Assert.AreEqual(true, sample.DoPrintUsage);
        }

        /**
         * A sample CommandLine class with a required option.
         */
        public class RequiredCommandLine : AbstractCommandLine {

            [CommandLineOption("string value", true, 's', "string")]
            public string StringValue { get; protected set; }

            public RequiredCommandLine(String[] args) 
            : base(args) 
            { }

            public RequiredCommandLine(String[] args, TextWriter writer)
            : base(args, writer) 
            { }
        }

        /**
         * Verify a required value is reported missing when it is not provided.
         */
        [Test()]
        public void TestCL_Required() {
            RequiredCommandLine sample = new RequiredCommandLine(
                new String[] { }
            );
            Assert.AreEqual(1, sample.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual('s', sample.MissingRequiredCommandLineOptions[0].ShortSymbol);
        }

        /**
         * Verify that a quoted and dashed argument value is properly assigned.
         */
        [Test()]
        public void TestCL_RequiredQuotedDash() {
            RequiredCommandLine sample = new RequiredCommandLine(
                new String[] { "-s", "foo-bar" }
            );
            Assert.AreEqual("foo-bar", sample.StringValue);
        }

        /**
         * A CommandLine class with a CommandLineOption on a property of a type that
         * is not supported.
         */
        public class UnsupportedCommandLine : AbstractCommandLine {

            [CommandLineOption("shouldn't work!", false, 'u', "unsupported",  "1/1/2001")]
            public DateTime Unsupported { get; protected set; }

            public UnsupportedCommandLine(String[] args) : base(args) { }

            public UnsupportedCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify an exception is thrown when trying to use a class with a CommandLineOption
         * attached to a property of an unsupported type.
         */
        [Test()]
        [ExpectedException(typeof(CommandLineException))]
        public void TestCL_Unsupported() {
            UnsupportedCommandLine sample = new UnsupportedCommandLine(
                new String[] { "-u", "whatevs" }
            );
        }

        /**
         * A CommandLine class that has a required value and a boolean flag.
         */
        public class RequiredWithFlagCommandLine : AbstractCommandLine {

            [CommandLineOption("a flag", false, 'f', "flag", "false")]
            public bool Flag { get; protected set; }

            [CommandLineOption("a string", true, 's', "string")]
            public string SomeString { get; protected set; }

            public RequiredWithFlagCommandLine(String[] args) : base(args) { }

            public RequiredWithFlagCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify that the flag and required value are set properly.
         */
        [Test()]
        public void TestCL_RequiredWithFlag() {
            RequiredWithFlagCommandLine sample1 = new RequiredWithFlagCommandLine(
                new String[] { "-s \"whatever\"", "-f" }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.IsTrue(sample1.Flag);
            Assert.AreEqual("whatever", sample1.SomeString);
        }

        /**
         * Verify that the flag and required value are set properly when they appear on the
         * command line in the opposite order.
         */
        [Test()]
        public void TestCL_RequiredWithFlagSwapped() {

            RequiredWithFlagCommandLine sample2 = new RequiredWithFlagCommandLine(
                new String[] { "-f", "-s \"whatever\"" }
            );
            Assert.AreEqual(0, sample2.MissingRequiredCommandLineOptions.Count);
            Assert.IsTrue(sample2.Flag);
            Assert.AreEqual("whatever", sample2.SomeString);
        }

        /**
         * A CommandLine class with a single optional int value.
         */
        public class OptionalIntValueCommandLine : AbstractCommandLine {

            [CommandLineOption("test int", false, 'i', "int")]
            public int TestInt { get; protected set; }

            public OptionalIntValueCommandLine(String[] args) : base(args) { }

            public OptionalIntValueCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify that an optional value is not reported as missing when it is not present.
         */
        [Test()]
        public void TestCL_OptionalIntValue() {
            TargetValueOptionalCommandLine sample1 = new TargetValueOptionalCommandLine(
                new String[] { }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
        }

        /**
         * A CommandLine class to test target value assignment.
         */
        public class TargetValueCommandLine : AbstractCommandLine {

            [CommandLineOption("a target string", true)]
            public string TargetString { get; protected set; }

            public TargetValueCommandLine(String[] args) : base(args) { }

            public TargetValueCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify that the target value is assigned in the simple use case.
         */
        [Test()]
        public void TestCL_TargetValueSimple() {
            TargetValueCommandLine sample1 = new TargetValueCommandLine(
                new String[] { "targetValue" }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual("targetValue", sample1.TargetString);
        }

        /**
         * Verify that the target value is reported as missing when it is not supplied.
         */
        [Test()]
        public void TestCL_TargetValueSimpleMissingRequired() {
            TargetValueCommandLine sample1 = new TargetValueCommandLine(
                new String[] { }
            );
            Assert.AreEqual(1, sample1.MissingRequiredCommandLineOptions.Count);
        }

        /**
         * A CommandLine class to test optional target values.
         */
        public class TargetValueOptionalCommandLine : AbstractCommandLine {

            [CommandLineOption("a target string", false, "default string")]
            public string TargetString { get; protected set; }

            public TargetValueOptionalCommandLine(String[] args) : base(args) { }

            public TargetValueOptionalCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify that no args are reported missing when an optional target value is not supplied.
         */
        [Test()]
        public void TestCL_TargetValueOptional() {
            TargetValueOptionalCommandLine sample1 = new TargetValueOptionalCommandLine(
                new String[] { }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual("default string", sample1.TargetString);
        }

        /**
         * A CommandLine class to test a bunch of args along with a target value.
         */
        public class TargetValueCompositeCommandLine : AbstractCommandLine {

            [CommandLineOption("a target string", true)]
            public string TargetString { get; protected set; }

            [CommandLineOption("test int", true, 'i', "int")]
            public int TestInt { get; protected set; }

            [CommandLineOption("optional boolean", false, 'o', "optional", "false")]
            public bool OptionalBool { get; protected set; }

            public TargetValueCompositeCommandLine(String[] args) : base(args) { }

            public TargetValueCompositeCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }
        }

        /**
         * Verify basic behavior of target args alongside other args.
         */
        [Test()]
        public void TestCL_TargetValueComposite() {
            TargetValueCompositeCommandLine sample1 = new TargetValueCompositeCommandLine(
                new String[] { "-i", "12345", "targetValue" }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual("targetValue", sample1.TargetString);
            Assert.AreEqual(12345, sample1.TestInt);
        }

        /**
         * Verify that a target value is properly assigned when preceded by another keyed arg.
         */
        [Test()]
        public void TestCL_TargetValueCompositeWithLeadingBool() {
            TargetValueCompositeCommandLine sample1 = new TargetValueCompositeCommandLine(
                new String[] { "-o", "-i", "12345", "targetValue" }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual("targetValue", sample1.TargetString);
            Assert.AreEqual(12345, sample1.TestInt);
            Assert.IsTrue(sample1.OptionalBool);
        }

        /**
         * Verify that a target value is properly assigned when preceded by a bool/flag argument.
         */
        [Test()]
        public void TestCL_TargetValueCompositeWithTrailingBool() {
            TargetValueCompositeCommandLine sample1 = new TargetValueCompositeCommandLine(
                new String[] { "-i", "12345", "-o", "targetValue" }
            );
            Assert.AreEqual(0, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual("targetValue", sample1.TargetString);
            Assert.AreEqual(12345, sample1.TestInt);
            Assert.IsTrue(sample1.OptionalBool);
        }

        /**
         * Verify that a target value is reported as missing when the last arg is a bool/flag arg.
         */
        [Test()]
        public void TestCL_TargetValueCompositeTrailingBoolMissingTarget() {
            TargetValueCompositeCommandLine sample1 = new TargetValueCompositeCommandLine(
                new String[] { "-i", "12345", "-o" }
            );
            Assert.AreEqual(1, sample1.MissingRequiredCommandLineOptions.Count);
            Assert.AreEqual(12345, sample1.TestInt);
            Assert.IsTrue(sample1.OptionalBool);
        }



        /**
         * A CommandLine class to test property validation.
         */
        public class ValidationCommandLine : AbstractCommandLine {

            private string stringValue;
            [CommandLineOption("string value", true)]
            public string StringValue {
                get { return this.stringValue; }
                set {
                    if ( ! value.Equals(value.ToUpper())) {
                        throw new Exception("Must be uppercase string!");
                    }
                    this.stringValue = value;
                }
            }

            public ValidationCommandLine(String[] args) : base(args) { }

            public ValidationCommandLine(String[] args, TextWriter writer)
            : base(args, writer) { }

        }

        /**
         * Verify that property-level validation short-circuits command line parsing. i.e. we get an exception if
         * the property rejects its assigned value.
         */
        [Test()]
        [ExpectedException(typeof(TargetInvocationException))]
        public void TestCL_ValidateExample() {
            ValidationCommandLine sample = new ValidationCommandLine(
                new String[] { "foobar" }
            );
        }

    }
}