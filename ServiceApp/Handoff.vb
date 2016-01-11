Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

<XmlRoot(ElementName:="Handoff", [Namespace]:="http://www.evolvi.biz")> _
Public Class Handoff
    <XmlElement(ElementName:="Agency")> _
    Public Property Agency As New List(Of Agency)

    <XmlElement(ElementName:="ImmediateDetail")> _
    Public Property ImmediateDetail As New List(Of ImmediateDetail)

    <XmlElement(ElementName:="FeeOrderRule")> _
    Public Property FeeOrderRule As New List(Of FeeOrderRule)

    <XmlAttribute(AttributeName:="schemaLocation", [Namespace]:="http://www.w3.org/2001/XMLSchema-instance")> _
    Public Property SchemaLocation As String

    <XmlAttribute(AttributeName:="SequenceNumber")> _
    Public Property SequenceNumber As String

    <XmlAttribute(AttributeName:="Created")> _
    Public Property Created As String

    <XmlAttribute(AttributeName:="Environment")> _
    Public Property Environment As String

    <XmlAttribute(AttributeName:="SchemaVersion")> _
    Public Property SchemaVersion As String

    <XmlAttribute(AttributeName:="OrderRef")> _
    Public Property OrderRef As String

    <XmlAttribute(AttributeName:="FulfilmentMethodType")> _
    Public Property FulfilmentMethodType As String

    <XmlAttribute(AttributeName:="DeliveryMethod")> _
    Public Property DeliveryMethod As String

    <XmlAttribute(AttributeName:="SignonToken")> _
    Public Property SignonToken As String
End Class

<XmlRoot(ElementName:="Agency")> _
Public Class Agency
    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="ExternalRef")> _
    Public Property ExternalRef As String
End Class

<XmlRoot(ElementName:="Account")> _
Public Class Account
    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="ExternalRef")> _
    Public Property ExternalRef As String

    <XmlAttribute(AttributeName:="Type")> _
    Public Property Type As String

    <XmlAttribute(AttributeName:="Period")> _
    Public Property Period As String

    <XmlAttribute(AttributeName:="Immediate")> _
    Public Property Immediate As String

    <XmlAttribute(AttributeName:="EvolviInvoice")> _
    Public Property EvolviInvoice As String

    <XmlAttribute(AttributeName:="EvolviPayment")> _
    Public Property EvolviPayment As String

    <XmlAttribute(AttributeName:="EvolviFinancial")> _
    Public Property EvolviFinancial As String
End Class

<XmlRoot(ElementName:="Person")> _
Public Class Person
    <XmlAttribute(AttributeName:="FirstName")> _
    Public Property FirstName As String

    <XmlAttribute(AttributeName:="LastName")> _
    Public Property LastName As String

    <XmlAttribute(AttributeName:="Title")> _
    Public Property Title As String
End Class

<XmlRoot(ElementName:="Email")> _
Public Class Email
    <XmlAttribute(AttributeName:="Address")> _
    Public Property Address As String
End Class

<XmlRoot(ElementName:="BookingAgent")> _
Public Class BookingAgent
    <XmlElement(ElementName:="Person")> _
    Public Property Person As New Person

    <XmlElement(ElementName:="Email")> _
    Public Property Email As New Email

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="ExternalRef")> _
    Public Property ExternalRef As String

    <XmlAttribute(AttributeName:="BranchExternalRef")> _
    Public Property BranchExternalRef As String

    <XmlAttribute(AttributeName:="Unit")> _
    Public Property Unit As String
End Class

<XmlRoot(ElementName:="TicketingAgent")> _
Public Class TicketingAgent
    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="ExternalRef")> _
    Public Property ExternalRef As String

    <XmlAttribute(AttributeName:="BranchExternalRef")> _
    Public Property BranchExternalRef As String

    <XmlAttribute(AttributeName:="Unit")> _
    Public Property Unit As String
End Class

<XmlRoot(ElementName:="CustomField")> _
Public Class CustomField
    <XmlAttribute(AttributeName:="Code")> _
    Public Property Code As String

    <XmlAttribute(AttributeName:="Label")> _
    Public Property Label As String

    <XmlAttribute(AttributeName:="Value")> _
    Public Property Value As String
End Class

<XmlRoot(ElementName:="Passenger")> _
Public Class Passenger
    <XmlElement(ElementName:="Person")> _
    Public Property Person As New Person

    <XmlElement(ElementName:="CustomField")> _
    Public Property CustomField As New List(Of CustomField)

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="AdultChild")> _
    Public Property AdultChild As String
End Class

<XmlRoot(ElementName:="PassengerGroup")> _
Public Class PassengerGroup
    <XmlElement(ElementName:="Passenger")> _
    Public Property Passenger As New List(Of Passenger)

    <XmlAttribute(AttributeName:="LeadRef")> _
    Public Property LeadRef As String
End Class

<XmlRoot(ElementName:="Phone")> _
Public Class Phone
    <XmlAttribute(AttributeName:="Number")> _
    Public Property Number As String
End Class

<XmlRoot(ElementName:="Address")> _
Public Class Address
    <XmlAttribute(AttributeName:="Organisation")> _
    Public Property Organisation As String

    <XmlAttribute(AttributeName:="Address1")> _
    Public Property Address1 As String

    <XmlAttribute(AttributeName:="Address2")> _
    Public Property Address2 As String

    <XmlAttribute(AttributeName:="City")> _
    Public Property City As String

    <XmlAttribute(AttributeName:="Postcode")> _
    Public Property Postcode As String

    <XmlAttribute(AttributeName:="Country")> _
    Public Property Country As String
End Class

<XmlRoot(ElementName:="AccountContact")> _
Public Class AccountContact
    <XmlElement(ElementName:="Person")> _
    Public Property Person As New Person

    <XmlElement(ElementName:="Phone")> _
    Public Property Phone As New Phone

    <XmlElement(ElementName:="Email")> _
    Public Property Email As New Email

    <XmlElement(ElementName:="Address")> _
    Public Property Address As New Address

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String
End Class

<XmlRoot(ElementName:="DeliveryContact")> _
Public Class DeliveryContact
    <XmlElement(ElementName:="Person")> _
    Public Property Person As New Person

    <XmlElement(ElementName:="Phone")> _
    Public Property Phone As New Phone

    <XmlElement(ElementName:="Email")> _
    Public Property Email As New Email

    <XmlElement(ElementName:="Address")> _
    Public Property Address As New Address

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String
End Class

<XmlRoot(ElementName:="Origin")> _
Public Class Origin
    <XmlAttribute(AttributeName:="UICCode")> _
    Public Property UicCode As String

    <XmlAttribute(AttributeName:="NLCCode")> _
    Public Property NlcCode As String

    <XmlAttribute(AttributeName:="CRSCode")> _
    Public Property CrsCode As String

    <XmlAttribute(AttributeName:="Name")> _
    Public Property Name As String
End Class

<XmlRoot(ElementName:="Destination")> _
Public Class Destination
    <XmlAttribute(AttributeName:="UICCode")> _
    Public Property UicCode As String

    <XmlAttribute(AttributeName:="NLCCode")> _
    Public Property NlcCode As String

    <XmlAttribute(AttributeName:="CRSCode")> _
    Public Property CrsCode As String

    <XmlAttribute(AttributeName:="Name")> _
    Public Property Name As String
End Class

<XmlRoot(ElementName:="Fare")> _
Public Class Fare
    <XmlAttribute(AttributeName:="TotalAmount")> _
    Public Property TotalAmount As String

    <XmlAttribute(AttributeName:="VATAmount")> _
    Public Property VatAmount As String

    <XmlAttribute(AttributeName:="VATCode")> _
    Public Property VatCode As String
End Class

<XmlRoot(ElementName:="Discount")> _
Public Class Discount
    <XmlAttribute(AttributeName:="TotalAmount")> _
    Public Property TotalAmount As String

    <XmlAttribute(AttributeName:="VATAmount")> _
    Public Property VatAmount As String

    <XmlAttribute(AttributeName:="VATCode")> _
    Public Property VatCode As String

    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="TransactionCharge")> _
Public Class TransactionCharge
    <XmlAttribute(AttributeName:="TotalAmount")> _
    Public Property TotalAmount As String

    <XmlAttribute(AttributeName:="VATAmount")> _
    Public Property VatAmount As String

    <XmlAttribute(AttributeName:="VATCode")> _
    Public Property VatCode As String
End Class

<XmlRoot(ElementName:="CreditCardCharge")> _
Public Class CreditCardCharge
    <XmlAttribute(AttributeName:="TotalAmount")> _
    Public Property TotalAmount As String

    <XmlAttribute(AttributeName:="VATAmount")> _
    Public Property VatAmount As String

    <XmlAttribute(AttributeName:="VATCode")> _
    Public Property VatCode As String
End Class

<XmlRoot(ElementName:="Sale")> _
Public Class Sale
    <XmlElement(ElementName:="Fare")> _
    Public Property Fare As New Fare

    <XmlElement(ElementName:="Discount")> _
    Public Property Discount As New Discount

    <XmlElement(ElementName:="TransactionCharge")> _
    Public Property TransactionCharge As New TransactionCharge

    <XmlElement(ElementName:="CreditCardCharge")> _
    Public Property CreditCardCharge As New CreditCardCharge
End Class

<XmlRoot(ElementName:="TransactionChargeItem")> _
Public Class TransactionChargeItem
    <XmlAttribute(AttributeName:="name")> _
    Public Property Name As String

    <XmlAttribute(AttributeName:="type")> _
    Public Property Type As String

    <XmlAttribute(AttributeName:="value")> _
    Public Property Value As String
End Class

<XmlRoot(ElementName:="TransactionChargeItemCollection")> _
Public Class TransactionChargeItemCollection
    <XmlElement(ElementName:="TransactionChargeItem")> _
    Public Property TransactionChargeItem As New List(Of TransactionChargeItem)
End Class

<XmlRoot(ElementName:="FareException")> _
Public Class FareException
    <XmlAttribute(AttributeName:="NormalFare")> _
    Public Property NormalFare As String

    <XmlAttribute(AttributeName:="OfferedFare")> _
    Public Property OfferedFare As String
End Class

<XmlRoot(ElementName:="Ticket")> _
Public Class Ticket
    <XmlElement(ElementName:="Sale")> _
    Public Property Sale As Sale

    <XmlElement(ElementName:="TransactionChargeItemCollection")> _
    Public Property TransactionChargeItemCollection As New TransactionChargeItemCollection

    <XmlElement(ElementName:="FareException")> _
    Public Property FareException As New FareException

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="PassengerRef")> _
    Public Property PassengerRef As String

    <XmlAttribute(AttributeName:="TCN")> _
    Public Property Tcn As String

    <XmlAttribute(AttributeName:="AdultChild")> _
    Public Property AdultChild As String

    <XmlAttribute(AttributeName:="Code")> _
    Public Property Code As String

    <XmlAttribute(AttributeName:="Name")> _
    Public Property Name As String

    <XmlAttribute(AttributeName:="Class")> _
    Public Property [Class] As String

    <XmlAttribute(AttributeName:="SingleReturn")> _
    Public Property SingleReturn As String

    <XmlAttribute(AttributeName:="RouteCode")> _
    Public Property RouteCode As String

    <XmlAttribute(AttributeName:="Route")> _
    Public Property Route As String
End Class

<XmlRoot(ElementName:="TOC")> _
Public Class Toc
    <XmlAttribute(AttributeName:="Code")> _
    Public Property Code As String

    <XmlAttribute(AttributeName:="Name")> _
    Public Property Name As String
End Class

<XmlRoot(ElementName:="TrainRoute")> _
Public Class TrainRoute
    <XmlElement(ElementName:="Origin")> _
    Public Property Origin As New Origin

    <XmlElement(ElementName:="Destination")> _
    Public Property Destination As New Destination

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="OriginDeparture")> _
    Public Property OriginDeparture As String

    <XmlAttribute(AttributeName:="DestinationArrival")> _
    Public Property DestinationArrival As String
End Class

<XmlRoot(ElementName:="Leg")> _
Public Class Leg
    <XmlElement(ElementName:="Origin")> _
    Public Property Origin As Origin

    <XmlElement(ElementName:="Destination")> _
    Public Property Destination As New Destination

    <XmlElement(ElementName:="TOC")> _
    Public Property Toc As Toc

    <XmlElement(ElementName:="TrainRoute")> _
    Public Property TrainRoute As New TrainRoute

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="Direction")> _
    Public Property Direction As String

    <XmlAttribute(AttributeName:="Departure")> _
    Public Property Departure As String

    <XmlAttribute(AttributeName:="Arrival")> _
    Public Property Arrival As String

    <XmlAttribute(AttributeName:="TransportMode")> _
    Public Property TransportMode As String

    <XmlElement(ElementName:="Reservation")> _
    Public Property Reservation As New List(Of Reservation)
End Class

<XmlRoot(ElementName:="Reservation")> _
Public Class Reservation
    <XmlAttribute(AttributeName:="PassengerRef")> _
    Public Property PassengerRef As String

    <XmlAttribute(AttributeName:="AccomodationUnit")> _
    Public Property AccomodationUnit As String
End Class

<XmlRoot(ElementName:="CarbonEmissions")> _
Public Class CarbonEmissions
    <XmlAttribute(AttributeName:="TransportType")> _
    Public Property TransportType As String

    <XmlAttribute(AttributeName:="Emissions")> _
    Public Property Emissions As String
End Class

<XmlRoot(ElementName:="CarbonEmissionDetails")> _
Public Class CarbonEmissionDetails
    <XmlElement(ElementName:="CarbonEmissions")> _
    Public Property CarbonEmissions As New List(Of CarbonEmissions)
End Class

<XmlRoot(ElementName:="Segment")> _
Public Class Segment
    <XmlElement(ElementName:="Origin")> _
    Public Property Origin As Origin

    <XmlElement(ElementName:="Destination")> _
    Public Property Destination As New Destination

    <XmlElement(ElementName:="Ticket")> _
    Public Property Ticket As List(Of Ticket)

    <XmlElement(ElementName:="Leg")> _
    Public Property Leg As New List(Of Leg)

    <XmlElement(ElementName:="CarbonEmissionDetails")> _
    Public Property CarbonEmissionDetails As New CarbonEmissionDetails

    <XmlAttribute(AttributeName:="Ref")> _
    Public Property Ref As String

    <XmlAttribute(AttributeName:="Distance")> _
    Public Property Distance As String

    <XmlAttribute(AttributeName:="DistanceUnits")> _
    Public Property DistanceUnits As String

    <XmlAttribute(AttributeName:="JourneyTime")> _
    Public Property JourneyTime As String
End Class

<XmlRoot(ElementName:="CollectionStation")> _
Public Class CollectionStation
    <XmlAttribute(AttributeName:="UICCode")> _
    Public Property UicCode As String

    <XmlAttribute(AttributeName:="NLCCode")> _
    Public Property NlcCode As String

    <XmlAttribute(AttributeName:="CRSCode")> _
    Public Property CrsCode As String

    <XmlAttribute(AttributeName:="Name")> _
    Public Property Name As String
End Class

<XmlRoot(ElementName:="TodDetails")> _
Public Class TodDetails
    <XmlElement(ElementName:="CollectionStation")> _
    Public Property CollectionStation As New CollectionStation

    <XmlAttribute(AttributeName:="Ctr")> _
    Public Property Ctr As String

    <XmlAttribute(AttributeName:="HelplinePhone")> _
    Public Property HelplinePhone As String
End Class

<XmlRoot(ElementName:="Value")> _
Public Class Value
    <XmlAttribute(AttributeName:="TotalAmount")> _
    Public Property TotalAmount As String

    <XmlAttribute(AttributeName:="VATAmount")> _
    Public Property VatAmount As String

    <XmlAttribute(AttributeName:="VATCode")> _
    Public Property VatCode As String
End Class

<XmlRoot(ElementName:="OrderItemFee")> _
Public Class OrderItemFee
    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="TicketFee")> _
Public Class TicketFee
    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="PassengerFee")> _
Public Class PassengerFee
    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="ExchangeFee")> _
Public Class ExchangeFee
    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="FeeOrderItemRule")> _
Public Class FeeOrderItemRule
    <XmlElement(ElementName:="FeeOrderItemRuleId")> _
    Public Property FeeOrderItemRuleId As String

    <XmlElement(ElementName:="Name")> _
    Public Property Name As String

    <XmlElement(ElementName:="Discount")> _
    Public Property Discount As New Discount

    <XmlElement(ElementName:="OrderItemFee")> _
    Public Property OrderItemFee As New OrderItemFee

    <XmlElement(ElementName:="TicketFee")> _
    Public Property TicketFee As New TicketFee

    <XmlElement(ElementName:="PassengerFee")> _
    Public Property PassengerFee As New PassengerFee

    <XmlElement(ElementName:="ExchangeFee")> _
    Public Property ExchangeFee As New ExchangeFee
End Class

<XmlRoot(ElementName:="ImmediateDetail")> _
Public Class ImmediateDetail
    <XmlElement(ElementName:="Account")> _
    Public Property Account As New Account

    <XmlElement(ElementName:="BookingAgent")> _
    Public Property BookingAgent As New BookingAgent

    <XmlElement(ElementName:="TicketingAgent")> _
    Public Property TicketingAgent As New TicketingAgent

    <XmlElement(ElementName:="PassengerGroup")> _
    Public Property PassengerGroup As New PassengerGroup

    <XmlElement(ElementName:="AccountContact")> _
    Public Property AccountContact As New AccountContact

    <XmlElement(ElementName:="DeliveryContact")> _
    Public Property DeliveryContact As New DeliveryContact

    <XmlElement(ElementName:="Segment")> _
    Public Property Segment As New List(Of Segment)

    <XmlElement(ElementName:="TodDetails")> _
    Public Property TodDetails As New TodDetails

    <XmlElement(ElementName:="FeeOrderItemRule")> _
    Public Property FeeOrderItemRule As New FeeOrderItemRule

    <XmlAttribute(AttributeName:="IssueRef")> _
    Public Property IssueRef As String

    <XmlAttribute(AttributeName:="TransactionType")> _
    Public Property TransactionType As String

    <XmlAttribute(AttributeName:="TransactionDate")> _
    Public Property TransactionDate As String

    <XmlAttribute(AttributeName:="MachineType")> _
    Public Property MachineType As String

    <XmlAttribute(AttributeName:="MachineNumber")> _
    Public Property MachineNumber As String

    <XmlAttribute(AttributeName:="CurrencyCode")> _
    Public Property CurrencyCode As String

    <XmlAttribute(AttributeName:="PersonalCCUsed")> _
    Public Property PersonalCcUsed As String

    <XmlAttribute(AttributeName:="FulfilmentType")> _
    Public Property FulfilmentType As String

End Class

<XmlRoot(ElementName:="OrderFee")> _
Public Class OrderFee
    <XmlElement(ElementName:="Value")> _
    Public Property Value As New Value

    <XmlElement(ElementName:="PercentageValue")> _
    Public Property PercentageValue As String

    <XmlElement(ElementName:="PercentageMinValue")> _
    Public Property PercentageMinValue As String

    <XmlElement(ElementName:="PercentageMaxValue")> _
    Public Property PercentageMaxValue As String
End Class

<XmlRoot(ElementName:="FeeOrderRule")> _
Public Class FeeOrderRule
    <XmlElement(ElementName:="FeeOrderRuleId")> _
    Public Property FeeOrderRuleId As String

    <XmlElement(ElementName:="Name")> _
    Public Property Name As String

    <XmlElement(ElementName:="OrderFee")> _
    Public Property OrderFee As New OrderFee
End Class