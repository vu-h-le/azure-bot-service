using System;
using Microsoft.Bot.Builder.FormFlow;

public enum ProjectType
{
    [Describe("Application Development")]
    AppDev = 1,
    [Describe("Service Fabric")]
    ASF,
    [Describe("DevOps")]
    DevOps,
    [Describe("Azure White Glove Onboarding")]
    Onboarding,
    [Describe("Big Data")]
    BigData,
    [Describe("Containers")]
    Containers,
    [Describe("Enterprise WordPress")]
    WordPress,
    [Describe("IoT")]
    IoT,
    [Describe("Open Source")]
    OSS,
    [Describe("Monetizing Data Strategy")]
    DataStrategy,
    Other
};
public enum YesNo { Yes = 1, No };
public enum Segment { Global = 1, EPG, CAM, CTM, Partner, ISV };

// For more information about this template visit http://aka.ms/azurebots-csharp-form
[Serializable]
[Template(TemplateUsage.EnumSelectOne,
          "Please select {&} {||}",
          ChoiceStyle = ChoiceStyleOptions.Carousel)]
[Template(TemplateUsage.NotUnderstood,
          "I do not understand \"{0}\".",
          "Try again, I don't get \"{0}\".")]
public class BasicForm
{
    [Prompt("Hi! What is your Microsoft email address?")]
    [Pattern(@"^[a-zA-Z0-9._%+-]+@(?i)microsoft.com$")]
    [Template(TemplateUsage.NotUnderstood,
              "That doesn't seem like a validate Microsoft email address.  Can you please try again?")]
    public string MicrosoftEmailAddress { get; set; }

    [Prompt("What is the {&}? {||}")]
    public string AccountName { get; set; }

    [Prompt("Is this a HiPo account? {||}")]
    public YesNo HiPo { get; set; }

    [Prompt("Please select the {&} {||}")]
    public Segment AccountSegment { get; set; }

    [Prompt("What is the {&}?")]
    public string ProjectedNetNewConsumption { get; set; }

    [Prompt("Where is the {&}?")]
    public string ProjectLocation { get; set; }

    [Prompt("Please describe the project.")]
    public string ProjectDescription { get; set; }

    [Prompt("Please select the {&} {||}")]
    public ProjectType ProjectType { get; set; }

    public static IForm<BasicForm> BuildForm()
    {
        // Builds an IForm<T> based on BasicForm
        return new FormBuilder<BasicForm>().Build();
    }

    public static IFormDialog<BasicForm> BuildFormDialog(FormOptions options = FormOptions.PromptInStart)
    {
        // Generated a new FormDialog<T> based on IForm<BasicForm>
        return FormDialog.FromForm(BuildForm, options);
    }
}
