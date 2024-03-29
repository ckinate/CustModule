using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Data
	{
		[JsonProperty("accountId")]
		public string AccountId { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("envelopeId")]
		public string EnvelopeId { get; set; }

		[JsonProperty("recipientId")]
		public string RecipientId { get; set; }

		[JsonProperty("envelopeSummary")]
		public EnvelopeSummary EnvelopeSummary { get; set; }
	}

	public class EnvelopeDocument
	{
		[JsonProperty("documentId")]
		public string DocumentId { get; set; }

		[JsonProperty("documentIdGuid")]
		public string DocumentIdGuid { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("uri")]
		public string Uri { get; set; }

		[JsonProperty("order")]
		public string Order { get; set; }

		[JsonProperty("pages")]
		public List<Page> Pages { get; set; }

		[JsonProperty("display")]
		public string Display { get; set; }

		[JsonProperty("includeInDownload")]
		public string IncludeInDownload { get; set; }

		[JsonProperty("signerMustAcknowledge")]
		public string SignerMustAcknowledge { get; set; }

		[JsonProperty("templateRequired")]
		public string TemplateRequired { get; set; }

		[JsonProperty("authoritativeCopy")]
		public string AuthoritativeCopy { get; set; }

		[JsonProperty("PDFBytes")]
		public string PDFBytes { get; set; }
	}

	public class EnvelopeMetadata
	{
		[JsonProperty("allowAdvancedCorrect")]
		public string AllowAdvancedCorrect { get; set; }

		[JsonProperty("enableSignWithNotary")]
		public string EnableSignWithNotary { get; set; }

		[JsonProperty("allowCorrect")]
		public string AllowCorrect { get; set; }
	}

	public class EnvelopeSummary
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("documentsUri")]
		public string DocumentsUri { get; set; }

		[JsonProperty("recipientsUri")]
		public string RecipientsUri { get; set; }

		[JsonProperty("attachmentsUri")]
		public string AttachmentsUri { get; set; }

		[JsonProperty("envelopeUri")]
		public string EnvelopeUri { get; set; }

		[JsonProperty("emailSubject")]
		public string EmailSubject { get; set; }

		[JsonProperty("envelopeId")]
		public string EnvelopeId { get; set; }

		[JsonProperty("signingLocation")]
		public string SigningLocation { get; set; }

		[JsonProperty("customFieldsUri")]
		public string CustomFieldsUri { get; set; }

		[JsonProperty("notificationUri")]
		public string NotificationUri { get; set; }

		[JsonProperty("enableWetSign")]
		public string EnableWetSign { get; set; }

		[JsonProperty("allowMarkup")]
		public string AllowMarkup { get; set; }

		[JsonProperty("allowReassign")]
		public string AllowReassign { get; set; }

		[JsonProperty("createdDateTime")]
		public DateTime? CreatedDateTime { get; set; }

		[JsonProperty("lastModifiedDateTime")]
		public DateTime? LastModifiedDateTime { get; set; }

		[JsonProperty("initialSentDateTime")]
		public DateTime? InitialSentDateTime { get; set; }

		[JsonProperty("sentDateTime")]
		public DateTime? SentDateTime { get; set; }

		[JsonProperty("statusChangedDateTime")]
		public DateTime? StatusChangedDateTime { get; set; }

		[JsonProperty("documentsCombinedUri")]
		public string DocumentsCombinedUri { get; set; }

		[JsonProperty("certificateUri")]
		public string CertificateUri { get; set; }

		[JsonProperty("templatesUri")]
		public string TemplatesUri { get; set; }

		[JsonProperty("expireEnabled")]
		public string ExpireEnabled { get; set; }

		[JsonProperty("expireDateTime")]
		public DateTime? ExpireDateTime { get; set; }

		[JsonProperty("expireAfter")]
		public string ExpireAfter { get; set; }

		[JsonProperty("sender")]
		public Sender Sender { get; set; }

		[JsonProperty("recipients")]
		public Recipients Recipients { get; set; }

		[JsonProperty("envelopeDocuments")]
		public List<EnvelopeDocument> EnvelopeDocuments { get; set; }

		[JsonProperty("purgeState")]
		public string PurgeState { get; set; }

		[JsonProperty("envelopeIdStamping")]
		public string EnvelopeIdStamping { get; set; }

		[JsonProperty("is21CFRPart11")]
		public string Is21CFRPart11 { get; set; }

		[JsonProperty("signerCanSignOnMobile")]
		public string SignerCanSignOnMobile { get; set; }

		[JsonProperty("autoNavigation")]
		public string AutoNavigation { get; set; }

		[JsonProperty("isSignatureProviderEnvelope")]
		public string IsSignatureProviderEnvelope { get; set; }

		[JsonProperty("hasFormDataChanged")]
		public string HasFormDataChanged { get; set; }

		[JsonProperty("allowComments")]
		public string AllowComments { get; set; }

		[JsonProperty("hasComments")]
		public string HasComments { get; set; }

		[JsonProperty("allowViewHistory")]
		public string AllowViewHistory { get; set; }

		[JsonProperty("envelopeMetadata")]
		public EnvelopeMetadata EnvelopeMetadata { get; set; }

		[JsonProperty("anySigner")]
		public object AnySigner { get; set; }

		[JsonProperty("envelopeLocation")]
		public string EnvelopeLocation { get; set; }

		[JsonProperty("isDynamicEnvelope")]
		public string IsDynamicEnvelope { get; set; }

		[JsonProperty("burnDefaultTabData")]
		public string BurnDefaultTabData { get; set; }
	}

	public class Page
	{
		[JsonProperty("pageId")]
		public string PageId { get; set; }

		[JsonProperty("sequence")]
		public string Sequence { get; set; }

		[JsonProperty("height")]
		public string Height { get; set; }

		[JsonProperty("width")]
		public string Width { get; set; }

		[JsonProperty("dpi")]
		public string Dpi { get; set; }
	}

	public class Recipients
	{
		[JsonProperty("signers")]
		public List<Signer> Signers { get; set; }

		[JsonProperty("agents")]
		public List<object> Agents { get; set; }

		[JsonProperty("editors")]
		public List<object> Editors { get; set; }

		[JsonProperty("intermediaries")]
		public List<object> Intermediaries { get; set; }

		[JsonProperty("carbonCopies")]
		public List<object> CarbonCopies { get; set; }

		[JsonProperty("certifiedDeliveries")]
		public List<object> CertifiedDeliveries { get; set; }

		[JsonProperty("inPersonSigners")]
		public List<object> InPersonSigners { get; set; }

		[JsonProperty("seals")]
		public List<object> Seals { get; set; }

		[JsonProperty("witnesses")]
		public List<object> Witnesses { get; set; }

		[JsonProperty("notaries")]
		public List<object> Notaries { get; set; }

		[JsonProperty("recipientCount")]
		public string RecipientCount { get; set; }

		[JsonProperty("currentRoutingOrder")]
		public string CurrentRoutingOrder { get; set; }
	}

	public class DocusignWebhookCompletedModel
	{
		[JsonProperty("event")]
		public string Event { get; set; }

		[JsonProperty("apiVersion")]
		public string ApiVersion { get; set; }

		[JsonProperty("uri")]
		public string Uri { get; set; }

		[JsonProperty("retryCount")]
		public int? RetryCount { get; set; }

		[JsonProperty("configurationId")]
		public int? ConfigurationId { get; set; }

		[JsonProperty("generatedDateTime")]
		public DateTime? GeneratedDateTime { get; set; }

		[JsonProperty("data")]
		public Data Data { get; set; }
	}

	public class Sender
	{
		[JsonProperty("userName")]
		public string UserName { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("accountId")]
		public string AccountId { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("ipAddress")]
		public string IpAddress { get; set; }
	}

	public class Signer
	{
		[JsonProperty("creationReason")]
		public string CreationReason { get; set; }

		[JsonProperty("isBulkRecipient")]
		public string IsBulkRecipient { get; set; }

		[JsonProperty("requireUploadSignature")]
		public string RequireUploadSignature { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("recipientId")]
		public string RecipientId { get; set; }

		[JsonProperty("recipientIdGuid")]
		public string RecipientIdGuid { get; set; }

		[JsonProperty("requireIdLookup")]
		public string RequireIdLookup { get; set; }

		[JsonProperty("userId")]
		public string UserId { get; set; }

		[JsonProperty("routingOrder")]
		public string RoutingOrder { get; set; }

		[JsonProperty("note")]
		public string Note { get; set; }

		[JsonProperty("roleName")]
		public string RoleName { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("completedCount")]
		public string CompletedCount { get; set; }

		[JsonProperty("signedDateTime")]
		public DateTime? SignedDateTime { get; set; }

		[JsonProperty("deliveredDateTime")]
		public DateTime? DeliveredDateTime { get; set; }

		[JsonProperty("sentDateTime")]
		public DateTime? SentDateTime { get; set; }

		[JsonProperty("deliveryMethod")]
		public string DeliveryMethod { get; set; }

		[JsonProperty("recipientType")]
		public string RecipientType { get; set; }

		[JsonProperty("recipientSuppliesTabs")]
		public string RecipientSuppliesTabs { get; set; }

		[JsonProperty("clientUserId")]
		public string ClientUserId { get; set; }
	}
}
