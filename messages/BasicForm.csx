using System;
using Microsoft.Bot.Builder.FormFlow;

public enum ProjectType
{
    ApplicationDevelopment = 1,
    AzureServiceFabric,
    DevOps,
    AzureWhiteGloveOnboarding,
    BigData,
    Containers,
    EnterpriseWordPress,
    InternetOfThings,
    OSS,
    MonetizingDataStrategy,
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
    [Prompt("Please provide your Microsoft email address so I can send emails to you after this process completes.")]
    [Pattern(@"^[a-zA-Z0-9._%+-]+@(?i)microsoft.com$")]
    [Template(TemplateUsage.NotUnderstood,
              "That doesn't seem like a valid Microsoft email address.  Can you please try again?")]
    public string MicrosoftEmailAddress { get; set; }

    [Prompt("Please provide the account name or TPID.")]
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
