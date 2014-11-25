using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMS_ASN1_Model;
using System.IO;
using org.bn;

namespace IEDExplorer
{
    public class MMSCapture
    {
        public enum CaptureDirection
        {
            In,
            Out
        }

        byte[] encodedPacket;
        MMSpdu pdu;
        CaptureDirection direction;
        DateTime time;
        public int PacketNr { get; set; }

        public MMSCapture(byte[] pkt, long pos, CaptureDirection dir)
        {
            encodedPacket = new Byte[pkt.Length - pos];
            Array.Copy(pkt, pos, encodedPacket, 0, pkt.Length - pos);
            direction = dir;
            time = DateTime.Now;
        }

        public MMSpdu MMSPdu { get { return pdu; } set { pdu = value; } }

        public DateTime Time { get { return time; } }

        public CaptureDirection Direction { get { return direction; } }

        public string XMLPdu
        {
            get
            {
                if (pdu != null)
                {
                    IEncoder xmlencoder = new IEDExplorer.BNExtension.XMLEncoder();

                    MemoryStream ms = new MemoryStream();
                    xmlencoder.encode<MMSpdu>(pdu, ms);
                    byte[] buf = ms.ToArray();
                    string st = System.Text.Encoding.ASCII.GetString(buf, 0, buf.Length);
                    return st;
                }
                else
                    return "No PDU available!";
            }
        }

        public string MMSPduType
        {
            get
            {
                if (pdu != null)
                {
                    if (pdu.isCancel_ErrorPDUSelected())
                        return "Cancel_ErrorPDU";
                    if (pdu.isCancel_RequestPDUSelected())
                        return "Cancel_RequestPDU";
                    if (pdu.isCancel_ResponsePDUSelected())
                        return "Cancel_ResponsePDU";
                    if (pdu.isConclude_ErrorPDUSelected())
                        return "Conclude_ErrorPDU";
                    if (pdu.isConclude_RequestPDUSelected())
                        return "Conclude_RequestPDU";
                    if (pdu.isConclude_ResponsePDUSelected())
                        return "Conclude_ResponsePDU";
                    if (pdu.isConfirmed_ErrorPDUSelected())
                        return "Confirmed_ErrorPDU";
                    if (pdu.isConfirmed_RequestPDUSelected())
                        return "Confirmed_RequestPDU";
                    if (pdu.isConfirmed_ResponsePDUSelected())
                        return "Confirmed_ResponsePDU";
                    if (pdu.isInitiate_ErrorPDUSelected())
                        return "Initiate_ErrorPDU";
                    if (pdu.isInitiate_RequestPDUSelected())
                        return "Initiate_RequestPDU";
                    if (pdu.isInitiate_ResponsePDUSelected())
                        return "Initiate_ResponsePDU";
                    if (pdu.isRejectPDUSelected())
                        return "RejectPDU";
                    if (pdu.isUnconfirmed_PDUSelected())
                        return "Unconfirmed_PDU";
                    return "Unknown PDU type!";
                }
                else
                    return "No PDU available!";
            }
        }

        public string MMSPduService
        {
            get
            {
                if (pdu != null)
                {
                    if (pdu.isConfirmed_RequestPDUSelected())
                        if (pdu.Confirmed_RequestPDU.Service != null)
                        {
                            if (pdu.Confirmed_RequestPDU.Service.isAcknowledgeEventNotificationSelected())
                                return "AcknowledgeEventNotification";
                            if (pdu.Confirmed_RequestPDU.Service.isAdditionalServiceSelected())
                                return "AdditionalService";
                            if (pdu.Confirmed_RequestPDU.Service.isAlterEventConditionMonitoringSelected())
                                return "AlterEventConditionMonitoring";
                            if (pdu.Confirmed_RequestPDU.Service.isAlterEventEnrollmentSelected())
                                return "AlterEventEnrollment";
                            if (pdu.Confirmed_RequestPDU.Service.isCreateJournalSelected())
                                return "CreateJournal";
                            if (pdu.Confirmed_RequestPDU.Service.isCreateProgramInvocationSelected())
                                return "CreateProgramInvocation";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineAccessControlListSelected())
                                return "DefineAccessControlList";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineEventActionSelected())
                                return "DefineEventAction";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineEventConditionSelected())
                                return "DefineEventCondition";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineEventEnrollmentSelected())
                                return "DefineEventEnrollment";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineNamedTypeSelected())
                                return "DefineNamedType";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineNamedVariableListSelected())
                                return "DefineNamedVariableList";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineNamedVariableSelected())
                                return "DefineNamedVariable";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineScatteredAccessSelected())
                                return "DefineScatteredAccess";
                            if (pdu.Confirmed_RequestPDU.Service.isDefineSemaphoreSelected())
                                return "DefineSemaphore";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteAccessControlListSelected())
                                return "DeleteAccessControlList";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteDomainSelected())
                                return "DeleteDomain";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteEventActionSelected())
                                return "DeleteEventAction";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteEventConditionSelected())
                                return "DeleteEventCondition";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteEventEnrollmentSelected())
                                return "DeleteEventEnrollment";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteJournalSelected())
                                return "DeleteJournal";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteNamedTypeSelected())
                                return "DeleteNamedType";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteNamedVariableListSelected())
                                return "DeleteNamedVariableList";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteProgramInvocationSelected())
                                return "DeleteProgramInvocation";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteSemaphoreSelected())
                                return "DeleteSemaphore";
                            if (pdu.Confirmed_RequestPDU.Service.isDeleteVariableAccessSelected())
                                return "DeleteVariableAccess";
                            if (pdu.Confirmed_RequestPDU.Service.isDownloadSegmentSelected())
                                return "DownloadSegment";
                            if (pdu.Confirmed_RequestPDU.Service.isExchangeDataSelected())
                                return "ExchangeData";
                            if (pdu.Confirmed_RequestPDU.Service.isFileCloseSelected())
                                return "FileClose";
                            if (pdu.Confirmed_RequestPDU.Service.isFileDeleteSelected())
                                return "FileDelete";
                            if (pdu.Confirmed_RequestPDU.Service.isFileDirectorySelected())
                                return "FileDirectory";
                            if (pdu.Confirmed_RequestPDU.Service.isFileOpenSelected())
                                return "FileOpen";
                            if (pdu.Confirmed_RequestPDU.Service.isFileReadSelected())
                                return "FileRead";
                            if (pdu.Confirmed_RequestPDU.Service.isFileRenameSelected())
                                return "FileRename";
                            if (pdu.Confirmed_RequestPDU.Service.isGetAccessControlListAttributesSelected())
                                return "GetAccessControlListAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetAlarmEnrollmentSummarySelected())
                                return "GetAlarmEnrollmentSummary";
                            if (pdu.Confirmed_RequestPDU.Service.isGetAlarmSummarySelected())
                                return "GetAlarmSummary";
                            if (pdu.Confirmed_RequestPDU.Service.isGetCapabilityListSelected())
                                return "GetCapabilityList";
                            if (pdu.Confirmed_RequestPDU.Service.isGetDataExchangeAttributesSelected())
                                return "GetDataExchangeAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetDomainAttributesSelected())
                                return "GetDomainAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetEventActionAttributesSelected())
                                return "GetEventActionAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetEventConditionAttributesSelected())
                                return "GetEventConditionAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetEventEnrollmentAttributesSelected())
                                return "GetEventEnrollmentAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetNamedTypeAttributesSelected())
                                return "GetNamedTypeAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetNamedVariableListAttributesSelected())
                                return "GetNamedVariableListAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetNameListSelected())
                                return "GetNameList";
                            if (pdu.Confirmed_RequestPDU.Service.isGetProgramInvocationAttributesSelected())
                                return "GetProgramInvocationAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetScatteredAccessAttributesSelected())
                                return "GetScatteredAccessAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isGetVariableAccessAttributesSelected())
                                return "GetVariableAccessAttributes";
                            if (pdu.Confirmed_RequestPDU.Service.isChangeAccessControlSelected())
                                return "ChangeAccessControl";
                            if (pdu.Confirmed_RequestPDU.Service.isIdentifySelected())
                                return "Identify";
                            if (pdu.Confirmed_RequestPDU.Service.isInitializeJournalSelected())
                                return "InitializeJournal";
                            if (pdu.Confirmed_RequestPDU.Service.isInitiateDownloadSequenceSelected())
                                return "InitiateDownloadSequence";
                            if (pdu.Confirmed_RequestPDU.Service.isInitiateUploadSequenceSelected())
                                return "InitiateUploadSequence";
                            if (pdu.Confirmed_RequestPDU.Service.isInputSelected())
                                return "Input";
                            if (pdu.Confirmed_RequestPDU.Service.isKillSelected())
                                return "Kill";
                            if (pdu.Confirmed_RequestPDU.Service.isLoadDomainContentSelected())
                                return "LoadDomainContent";
                            if (pdu.Confirmed_RequestPDU.Service.isObtainFileSelected())
                                return "ObtainFile";
                            if (pdu.Confirmed_RequestPDU.Service.isOutputSelected())
                                return "Output";
                            if (pdu.Confirmed_RequestPDU.Service.isReadJournalSelected())
                                return "ReadJournal";
                            if (pdu.Confirmed_RequestPDU.Service.isReadSelected())
                                return "Read";
                            if (pdu.Confirmed_RequestPDU.Service.isRelinquishControlSelected())
                                return "RelinquishControl";
                            if (pdu.Confirmed_RequestPDU.Service.isRenameSelected())
                                return "Rename";
                            if (pdu.Confirmed_RequestPDU.Service.isReportAccessControlledObjectsSelected())
                                return "ReportAccessControlledObjects";
                            if (pdu.Confirmed_RequestPDU.Service.isReportEventActionStatusSelected())
                                return "ReportEventActionStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportEventConditionStatusSelected())
                                return "ReportEventConditionStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportEventEnrollmentStatusSelected())
                                return "ReportEventEnrollmentStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportJournalStatusSelected())
                                return "ReportJournalStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportPoolSemaphoreStatusSelected())
                                return "ReportPoolSemaphoreStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportSemaphoreEntryStatusSelected())
                                return "ReportSemaphoreEntryStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isReportSemaphoreStatusSelected())
                                return "ReportSemaphoreStatus";
                            if (pdu.Confirmed_RequestPDU.Service.isRequestDomainDownloadSelected())
                                return "RequestDomainDownload";
                            if (pdu.Confirmed_RequestPDU.Service.isRequestDomainUploadSelected())
                                return "RequestDomainUpload";
                            if (pdu.Confirmed_RequestPDU.Service.isResetSelected())
                                return "Reset";
                            if (pdu.Confirmed_RequestPDU.Service.isResumeSelected())
                                return "Resume";
                            if (pdu.Confirmed_RequestPDU.Service.isStartSelected())
                                return "Start";
                            if (pdu.Confirmed_RequestPDU.Service.isStatusSelected())
                                return "Status";
                            if (pdu.Confirmed_RequestPDU.Service.isStopSelected())
                                return "Stop";
                            if (pdu.Confirmed_RequestPDU.Service.isStoreDomainContentSelected())
                                return "StoreDomainContent";
                            if (pdu.Confirmed_RequestPDU.Service.isTakeControlSelected())
                                return "TakeControl";
                            if (pdu.Confirmed_RequestPDU.Service.isTerminateDownloadSequenceSelected())
                                return "TerminateDownloadSequence";
                            if (pdu.Confirmed_RequestPDU.Service.isTerminateUploadSequenceSelected())
                                return "TerminateUploadSequence";
                            if (pdu.Confirmed_RequestPDU.Service.isTriggerEventSelected())
                                return "TriggerEvent";
                            if (pdu.Confirmed_RequestPDU.Service.isUploadSegmentSelected())
                                return "UploadSegment";
                            if (pdu.Confirmed_RequestPDU.Service.isWriteJournalSelected())
                                return "WriteJournal";
                            if (pdu.Confirmed_RequestPDU.Service.isWriteSelected())
                                return "Write";
                        }
                        else
                            return "Service not found!";
                    if (pdu.isConfirmed_ResponsePDUSelected())
                        if (pdu.Confirmed_ResponsePDU.Service != null)
                        {
                            if (pdu.Confirmed_ResponsePDU.Service.isAcknowledgeEventNotificationSelected())
                                return "AcknowledgeEventNotification";
                            if (pdu.Confirmed_ResponsePDU.Service.isAdditionalServiceSelected())
                                return "AdditionalService";
                            if (pdu.Confirmed_ResponsePDU.Service.isAlterEventConditionMonitoringSelected())
                                return "AlterEventConditionMonitoring";
                            if (pdu.Confirmed_ResponsePDU.Service.isAlterEventEnrollmentSelected())
                                return "AlterEventEnrollment";
                            if (pdu.Confirmed_ResponsePDU.Service.isCreateJournalSelected())
                                return "CreateJournal";
                            if (pdu.Confirmed_ResponsePDU.Service.isCreateProgramInvocationSelected())
                                return "CreateProgramInvocation";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineAccessControlListSelected())
                                return "DefineAccessControlList";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineEventActionSelected())
                                return "DefineEventAction";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineEventConditionSelected())
                                return "DefineEventCondition";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineEventEnrollmentSelected())
                                return "DefineEventEnrollment";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineNamedTypeSelected())
                                return "DefineNamedType";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineNamedVariableListSelected())
                                return "DefineNamedVariableList";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineNamedVariableSelected())
                                return "DefineNamedVariable";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineScatteredAccessSelected())
                                return "DefineScatteredAccess";
                            if (pdu.Confirmed_ResponsePDU.Service.isDefineSemaphoreSelected())
                                return "DefineSemaphore";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteAccessControlListSelected())
                                return "DeleteAccessControlList";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteDomainSelected())
                                return "DeleteDomain";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteEventActionSelected())
                                return "DeleteEventAction";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteEventConditionSelected())
                                return "DeleteEventCondition";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteEventEnrollmentSelected())
                                return "DeleteEventEnrollment";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteJournalSelected())
                                return "DeleteJournal";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteNamedTypeSelected())
                                return "DeleteNamedType";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteNamedVariableListSelected())
                                return "DeleteNamedVariableList";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteProgramInvocationSelected())
                                return "DeleteProgramInvocation";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteSemaphoreSelected())
                                return "DeleteSemaphore";
                            if (pdu.Confirmed_ResponsePDU.Service.isDeleteVariableAccessSelected())
                                return "DeleteVariableAccess";
                            if (pdu.Confirmed_ResponsePDU.Service.isDownloadSegmentSelected())
                                return "DownloadSegment";
                            if (pdu.Confirmed_ResponsePDU.Service.isExchangeDataSelected())
                                return "ExchangeData";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileCloseSelected())
                                return "FileClose";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileDeleteSelected())
                                return "FileDelete";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileDirectorySelected())
                                return "FileDirectory";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileOpenSelected())
                                return "FileOpen";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileReadSelected())
                                return "FileRead";
                            if (pdu.Confirmed_ResponsePDU.Service.isFileRenameSelected())
                                return "FileRename";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetAccessControlListAttributesSelected())
                                return "GetAccessControlListAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetAlarmEnrollmentSummarySelected())
                                return "GetAlarmEnrollmentSummary";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetAlarmSummarySelected())
                                return "GetAlarmSummary";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetCapabilityListSelected())
                                return "GetCapabilityList";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetDataExchangeAttributesSelected())
                                return "GetDataExchangeAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetDomainAttributesSelected())
                                return "GetDomainAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetEventActionAttributesSelected())
                                return "GetEventActionAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetEventConditionAttributesSelected())
                                return "GetEventConditionAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetEventEnrollmentAttributesSelected())
                                return "GetEventEnrollmentAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetNamedTypeAttributesSelected())
                                return "GetNamedTypeAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetNamedVariableListAttributesSelected())
                                return "GetNamedVariableListAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetNameListSelected())
                                return "GetNameList";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetProgramInvocationAttributesSelected())
                                return "GetProgramInvocationAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetScatteredAccessAttributesSelected())
                                return "GetScatteredAccessAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isGetVariableAccessAttributesSelected())
                                return "GetVariableAccessAttributes";
                            if (pdu.Confirmed_ResponsePDU.Service.isChangeAccessControlSelected())
                                return "ChangeAccessControl";
                            if (pdu.Confirmed_ResponsePDU.Service.isIdentifySelected())
                                return "Identify";
                            if (pdu.Confirmed_ResponsePDU.Service.isInitializeJournalSelected())
                                return "InitializeJournal";
                            if (pdu.Confirmed_ResponsePDU.Service.isInitiateDownloadSequenceSelected())
                                return "InitiateDownloadSequence";
                            if (pdu.Confirmed_ResponsePDU.Service.isInitiateUploadSequenceSelected())
                                return "InitiateUploadSequence";
                            if (pdu.Confirmed_ResponsePDU.Service.isInputSelected())
                                return "Input";
                            if (pdu.Confirmed_ResponsePDU.Service.isKillSelected())
                                return "Kill";
                            if (pdu.Confirmed_ResponsePDU.Service.isLoadDomainContentSelected())
                                return "LoadDomainContent";
                            if (pdu.Confirmed_ResponsePDU.Service.isObtainFileSelected())
                                return "ObtainFile";
                            if (pdu.Confirmed_ResponsePDU.Service.isOutputSelected())
                                return "Output";
                            if (pdu.Confirmed_ResponsePDU.Service.isReadJournalSelected())
                                return "ReadJournal";
                            if (pdu.Confirmed_ResponsePDU.Service.isReadSelected())
                                return "Read";
                            if (pdu.Confirmed_ResponsePDU.Service.isRelinquishControlSelected())
                                return "RelinquishControl";
                            if (pdu.Confirmed_ResponsePDU.Service.isRenameSelected())
                                return "Rename";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportAccessControlledObjectsSelected())
                                return "ReportAccessControlledObjects";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportEventActionStatusSelected())
                                return "ReportEventActionStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportEventConditionStatusSelected())
                                return "ReportEventConditionStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportEventEnrollmentStatusSelected())
                                return "ReportEventEnrollmentStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportJournalStatusSelected())
                                return "ReportJournalStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportPoolSemaphoreStatusSelected())
                                return "ReportPoolSemaphoreStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportSemaphoreEntryStatusSelected())
                                return "ReportSemaphoreEntryStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isReportSemaphoreStatusSelected())
                                return "ReportSemaphoreStatus";
                            if (pdu.Confirmed_ResponsePDU.Service.isRequestDomainDownloadSelected())
                                return "ResponseDomainDownload";
                            if (pdu.Confirmed_ResponsePDU.Service.isRequestDomainUploadSelected())
                                return "ResponseDomainUpload";
                            if (pdu.Confirmed_ResponsePDU.Service.isResetSelected())
                                return "Reset";
                            if (pdu.Confirmed_ResponsePDU.Service.isResumeSelected())
                                return "Resume";
                            if (pdu.Confirmed_ResponsePDU.Service.isStartSelected())
                                return "Start";
                            if (pdu.Confirmed_ResponsePDU.Service.isStatusSelected())
                                return "Status";
                            if (pdu.Confirmed_ResponsePDU.Service.isStopSelected())
                                return "Stop";
                            if (pdu.Confirmed_ResponsePDU.Service.isStoreDomainContentSelected())
                                return "StoreDomainContent";
                            if (pdu.Confirmed_ResponsePDU.Service.isTakeControlSelected())
                                return "TakeControl";
                            if (pdu.Confirmed_ResponsePDU.Service.isTerminateDownloadSequenceSelected())
                                return "TerminateDownloadSequence";
                            if (pdu.Confirmed_ResponsePDU.Service.isTerminateUploadSequenceSelected())
                                return "TerminateUploadSequence";
                            if (pdu.Confirmed_ResponsePDU.Service.isTriggerEventSelected())
                                return "TriggerEvent";
                            if (pdu.Confirmed_ResponsePDU.Service.isUploadSegmentSelected())
                                return "UploadSegment";
                            if (pdu.Confirmed_ResponsePDU.Service.isWriteJournalSelected())
                                return "WriteJournal";
                            if (pdu.Confirmed_ResponsePDU.Service.isWriteSelected())
                                return "Write";
                        }
                        else
                            return "Service not found!";
                    if (pdu.isUnconfirmed_PDUSelected())
                        if (pdu.Unconfirmed_PDU.Service != null)
                        {
                            if (pdu.Unconfirmed_PDU.Service.isEventNotificationSelected())
                                return "EventNotification";
                            if (pdu.Unconfirmed_PDU.Service.isInformationReportSelected())
                                return "InformationReport";
                            if (pdu.Unconfirmed_PDU.Service.isUnsolicitedStatusSelected())
                                return "UnsolicitedStatus";
                        }
                        else
                            return "Service not found!";
                    return "PDU type without service";
                }
                else
                    return "No PDU available!";
            }
        }

        /*public string Packet
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (byte b in encodedPacket)
                {
                    sb.Append(b.ToString("X2"));
                    sb.Append(' ');
                }
                return sb.ToString();
            }
        }*/

        public byte[] EncodedPacket { get { return encodedPacket; } }
    }
}
