Imports EvoDatabaseUtils
Imports EvoUtilities.ConfigUtils

Partial Public Class Npsa

    Public Sub New( _
        ByVal pBatch_id As String, _
        ByVal pInterface As String, _
        ByVal pVoucher_type As String, _
        ByVal pTrans_type As String, _
        ByVal pClient As String, _
        ByVal pAccount As String, _
        ByVal pDim_1 As String, _
        ByVal pDim_2 As String, _
        ByVal pDim_3 As String, _
        ByVal pDim_4 As String, _
        ByVal pDim_5 As String, _
        ByVal pDim_6 As String, _
        ByVal pDim_7 As String, _
        ByVal pTax_code As String, _
        ByVal pTax_system As String, _
        ByVal pCurrency As String, _
        ByVal pDc_flag As String, _
        ByVal pCur_amount As String, _
        ByVal pAmount As String, _
        ByVal pNumber_1 As String, _
        ByVal pValue_1 As String, _
        ByVal pValue_2 As String, _
        ByVal pValue_3 As String, _
        ByVal pDescript As String, _
        ByVal pTrans_date As String, _
        ByVal pVoucher_date As String, _
        ByVal pVoucher_no As String, _
        ByVal pPeriod As String, _
        ByVal pTax_id As String, _
        ByVal pExt_inv_ref As String, _
        ByVal pExt_ref As String, _
        ByVal pDue_date As String, _
        ByVal pDisc_date As String, _
        ByVal pDiscount As String, _
        ByVal pCommitment As String, _
        ByVal pOrder_id As String, _
        ByVal pKid As String, _
        ByVal pPay_transfer As String, _
        ByVal pStatus As String, _
        ByVal pApar_type As String, _
        ByVal pApar_id As String, _
        ByVal pPay_flag As String, _
        ByVal pVoucher_ref As String, _
        ByVal pSequence_ref As String, _
        ByVal pIntrule_id As String, _
        ByVal pFactor_short As String, _
        ByVal pResponsible As String, _
        ByVal pApar_name As String, _
        ByVal pAddress As String, _
        ByVal pProvince As String, _
        ByVal pPlace As String, _
        ByVal pBank_account As String, _
        ByVal pPay_method As String, _
        ByVal pVat_reg_no As String, _
        ByVal pZip_code As String, _
        ByVal pCurr_licence As String, _
        ByVal pAccount2 As String, _
        ByVal pBase_amount As String, _
        ByVal pBase_curr As String, _
        ByVal pPay_temp_id As String, _
        ByVal pAllocation_key As String, _
        ByVal pPeriod_no As String, _
        ByVal pClearing_code As String, _
        ByVal pSwift As String, _
        ByVal pArrive_id As String, _
        ByVal pBank_acc_type As String)
        mBatch_id = pBatch_id
        mInterface = pInterface
        mVoucher_type = pVoucher_type
        mTrans_type = pTrans_type
        mClient = pClient
        mAccount = pAccount
        mDim_1 = pDim_1
        mDim_2 = pDim_2
        mDim_3 = pDim_3
        mDim_4 = pDim_4
        mDim_5 = pDim_5
        mDim_6 = pDim_6
        mDim_7 = pDim_7
        mTax_code = pTax_code
        mTax_system = pTax_system
        mCurrency = pCurrency
        mDc_flag = pDc_flag
        mCur_amount = pCur_amount
        mAmount = pAmount
        mNumber_1 = pNumber_1
        mValue_1 = pValue_1
        mValue_2 = pValue_2
        mValue_3 = pValue_3
        mDescript = pDescript
        mTrans_date = pTrans_date
        mVoucher_date = pVoucher_date
        mVoucher_no = pVoucher_no
        mPeriod = pPeriod
        mTax_id = pTax_id
        mExt_inv_ref = pExt_inv_ref
        mExt_ref = pExt_ref
        mDue_date = pDue_date
        mDisc_date = pDisc_date
        mDiscount = pDiscount
        mCommitment = pCommitment
        mOrder_id = pOrder_id
        mKid = pKid
        mPay_transfer = pPay_transfer
        mStatus = pStatus
        mApar_type = pApar_type
        mApar_id = pApar_id
        mPay_flag = pPay_flag
        mVoucher_ref = pVoucher_ref
        mSequence_ref = pSequence_ref
        mIntrule_id = pIntrule_id
        mFactor_short = pFactor_short
        mResponsible = pResponsible
        mApar_name = pApar_name
        mAddress = pAddress
        mProvince = pProvince
        mPlace = pPlace
        mBank_account = pBank_account
        mPay_method = pPay_method
        mVat_reg_no = pVat_reg_no
        mZip_code = pZip_code
        mCurr_licence = pCurr_licence
        mAccount2 = pAccount2
        mBase_amount = pBase_amount
        mBase_curr = pBase_curr
        mPay_temp_id = pPay_temp_id
        mAllocation_key = pAllocation_key
        mPeriod_no = pPeriod_no
        mClearing_code = pClearing_code
        mSwift = pSwift
        mArrive_id = pArrive_id
        mBank_acc_type = pBank_acc_type
    End Sub

    Public Sub New( _
)
    End Sub

    Private mBatch_id As String
    Private mInterface As String
    Private mVoucher_type As String
    Private mTrans_type As String
    Private mClient As String
    Private mAccount As String
    Private mDim_1 As String
    Private mDim_2 As String
    Private mDim_3 As String
    Private mDim_4 As String
    Private mDim_5 As String
    Private mDim_6 As String
    Private mDim_7 As String
    Private mTax_code As String
    Private mTax_system As String
    Private mCurrency As String
    Private mDc_flag As String
    Private mCur_amount As String
    Private mAmount As String
    Private mNumber_1 As String
    Private mValue_1 As String
    Private mValue_2 As String
    Private mValue_3 As String
    Private mDescript As String
    Private mTrans_date As String
    Private mVoucher_date As String
    Private mVoucher_no As String
    Private mPeriod As String
    Private mTax_id As String
    Private mExt_inv_ref As String
    Private mExt_ref As String
    Private mDue_date As String
    Private mDisc_date As String
    Private mDiscount As String
    Private mCommitment As String
    Private mOrder_id As String
    Private mKid As String
    Private mPay_transfer As String
    Private mStatus As String
    Private mApar_type As String
    Private mApar_id As String
    Private mPay_flag As String
    Private mVoucher_ref As String
    Private mSequence_ref As String
    Private mIntrule_id As String
    Private mFactor_short As String
    Private mResponsible As String
    Private mApar_name As String
    Private mAddress As String
    Private mProvince As String
    Private mPlace As String
    Private mBank_account As String
    Private mPay_method As String
    Private mVat_reg_no As String
    Private mZip_code As String
    Private mCurr_licence As String
    Private mAccount2 As String
    Private mBase_amount As String
    Private mBase_curr As String
    Private mPay_temp_id As String
    Private mAllocation_key As String
    Private mPeriod_no As String
    Private mClearing_code As String
    Private mSwift As String
    Private mArrive_id As String
    Private mBank_acc_type As String

    Public Property Batch_id() As String
        Get
            Return mBatch_id
        End Get
        Set(ByVal value As String)
            mBatch_id = value
        End Set
    End Property

    Public Property [Interface]() As String
        Get
            Return mInterface
        End Get
        Set(ByVal value As String)
            mInterface = value
        End Set
    End Property

    Public Property Voucher_type() As String
        Get
            Return mVoucher_type
        End Get
        Set(ByVal value As String)
            mVoucher_type = value
        End Set
    End Property

    Public Property Trans_type() As String
        Get
            Return mTrans_type
        End Get
        Set(ByVal value As String)
            mTrans_type = value
        End Set
    End Property

    Public Property Client() As String
        Get
            Return mClient
        End Get
        Set(ByVal value As String)
            mClient = value
        End Set
    End Property

    Public Property Account() As String
        Get
            Return mAccount
        End Get
        Set(ByVal value As String)
            mAccount = value
        End Set
    End Property

    Public Property Dim_1() As String
        Get
            Return mDim_1
        End Get
        Set(ByVal value As String)
            mDim_1 = value
        End Set
    End Property

    Public Property Dim_2() As String
        Get
            Return mDim_2
        End Get
        Set(ByVal value As String)
            mDim_2 = value
        End Set
    End Property

    Public Property Dim_3() As String
        Get
            Return mDim_3
        End Get
        Set(ByVal value As String)
            mDim_3 = value
        End Set
    End Property

    Public Property Dim_4() As String
        Get
            Return mDim_4
        End Get
        Set(ByVal value As String)
            mDim_4 = value
        End Set
    End Property

    Public Property Dim_5() As String
        Get
            Return mDim_5
        End Get
        Set(ByVal value As String)
            mDim_5 = value
        End Set
    End Property

    Public Property Dim_6() As String
        Get
            Return mDim_6
        End Get
        Set(ByVal value As String)
            mDim_6 = value
        End Set
    End Property

    Public Property Dim_7() As String
        Get
            Return mDim_7
        End Get
        Set(ByVal value As String)
            mDim_7 = value
        End Set
    End Property

    Public Property Tax_code() As String
        Get
            Return mTax_code
        End Get
        Set(ByVal value As String)
            mTax_code = value
        End Set
    End Property

    Public Property Tax_system() As String
        Get
            Return mTax_system
        End Get
        Set(ByVal value As String)
            mTax_system = value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return mCurrency
        End Get
        Set(ByVal value As String)
            mCurrency = value
        End Set
    End Property

    Public Property Dc_flag() As String
        Get
            Return mDc_flag
        End Get
        Set(ByVal value As String)
            mDc_flag = value
        End Set
    End Property

    Public Property Cur_amount() As String
        Get
            Return mCur_amount
        End Get
        Set(ByVal value As String)
            mCur_amount = value
        End Set
    End Property

    Public Property Amount() As String
        Get
            Return mAmount
        End Get
        Set(ByVal value As String)
            mAmount = value
        End Set
    End Property

    Public Property Number_1() As String
        Get
            Return mNumber_1
        End Get
        Set(ByVal value As String)
            mNumber_1 = value
        End Set
    End Property

    Public Property Value_1() As String
        Get
            Return mValue_1
        End Get
        Set(ByVal value As String)
            mValue_1 = value
        End Set
    End Property

    Public Property Value_2() As String
        Get
            Return mValue_2
        End Get
        Set(ByVal value As String)
            mValue_2 = value
        End Set
    End Property

    Public Property Value_3() As String
        Get
            Return mValue_3
        End Get
        Set(ByVal value As String)
            mValue_3 = value
        End Set
    End Property

    Public Property Descript() As String
        Get
            Return mDescript
        End Get
        Set(ByVal value As String)
            mDescript = value
        End Set
    End Property

    Public Property Trans_date() As String
        Get
            Return mTrans_date
        End Get
        Set(ByVal value As String)
            mTrans_date = value
        End Set
    End Property

    Public Property Voucher_date() As String
        Get
            Return mVoucher_date
        End Get
        Set(ByVal value As String)
            mVoucher_date = value
        End Set
    End Property

    Public Property Voucher_no() As String
        Get
            Return mVoucher_no
        End Get
        Set(ByVal value As String)
            mVoucher_no = value
        End Set
    End Property

    Public Property Period() As String
        Get
            Return mPeriod
        End Get
        Set(ByVal value As String)
            mPeriod = value
        End Set
    End Property

    Public Property Tax_id() As String
        Get
            Return mTax_id
        End Get
        Set(ByVal value As String)
            mTax_id = value
        End Set
    End Property

    Public Property Ext_inv_ref() As String
        Get
            Return mExt_inv_ref
        End Get
        Set(ByVal value As String)
            mExt_inv_ref = value
        End Set
    End Property

    Public Property Ext_ref() As String
        Get
            Return mExt_ref
        End Get
        Set(ByVal value As String)
            mExt_ref = value
        End Set
    End Property

    Public Property Due_date() As String
        Get
            Return mDue_date
        End Get
        Set(ByVal value As String)
            mDue_date = value
        End Set
    End Property

    Public Property Disc_date() As String
        Get
            Return mDisc_date
        End Get
        Set(ByVal value As String)
            mDisc_date = value
        End Set
    End Property

    Public Property Discount() As String
        Get
            Return mDiscount
        End Get
        Set(ByVal value As String)
            mDiscount = value
        End Set
    End Property

    Public Property Commitment() As String
        Get
            Return mCommitment
        End Get
        Set(ByVal value As String)
            mCommitment = value
        End Set
    End Property

    Public Property Order_id() As String
        Get
            Return mOrder_id
        End Get
        Set(ByVal value As String)
            mOrder_id = value
        End Set
    End Property

    Public Property Kid() As String
        Get
            Return mKid
        End Get
        Set(ByVal value As String)
            mKid = value
        End Set
    End Property

    Public Property Pay_transfer() As String
        Get
            Return mPay_transfer
        End Get
        Set(ByVal value As String)
            mPay_transfer = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return mStatus
        End Get
        Set(ByVal value As String)
            mStatus = value
        End Set
    End Property

    Public Property Apar_type() As String
        Get
            Return mApar_type
        End Get
        Set(ByVal value As String)
            mApar_type = value
        End Set
    End Property

    Public Property Apar_id() As String
        Get
            Return mApar_id
        End Get
        Set(ByVal value As String)
            mApar_id = value
        End Set
    End Property

    Public Property Pay_flag() As String
        Get
            Return mPay_flag
        End Get
        Set(ByVal value As String)
            mPay_flag = value
        End Set
    End Property

    Public Property Voucher_ref() As String
        Get
            Return mVoucher_ref
        End Get
        Set(ByVal value As String)
            mVoucher_ref = value
        End Set
    End Property

    Public Property Sequence_ref() As String
        Get
            Return mSequence_ref
        End Get
        Set(ByVal value As String)
            mSequence_ref = value
        End Set
    End Property

    Public Property Intrule_id() As String
        Get
            Return mIntrule_id
        End Get
        Set(ByVal value As String)
            mIntrule_id = value
        End Set
    End Property

    Public Property Factor_short() As String
        Get
            Return mFactor_short
        End Get
        Set(ByVal value As String)
            mFactor_short = value
        End Set
    End Property

    Public Property Responsible() As String
        Get
            Return mResponsible
        End Get
        Set(ByVal value As String)
            mResponsible = value
        End Set
    End Property

    Public Property Apar_name() As String
        Get
            Return mApar_name
        End Get
        Set(ByVal value As String)
            mApar_name = value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return mAddress
        End Get
        Set(ByVal value As String)
            mAddress = value
        End Set
    End Property

    Public Property Province() As String
        Get
            Return mProvince
        End Get
        Set(ByVal value As String)
            mProvince = value
        End Set
    End Property

    Public Property Place() As String
        Get
            Return mPlace
        End Get
        Set(ByVal value As String)
            mPlace = value
        End Set
    End Property

    Public Property Bank_account() As String
        Get
            Return mBank_account
        End Get
        Set(ByVal value As String)
            mBank_account = value
        End Set
    End Property

    Public Property Pay_method() As String
        Get
            Return mPay_method
        End Get
        Set(ByVal value As String)
            mPay_method = value
        End Set
    End Property

    Public Property Vat_reg_no() As String
        Get
            Return mVat_reg_no
        End Get
        Set(ByVal value As String)
            mVat_reg_no = value
        End Set
    End Property

    Public Property Zip_code() As String
        Get
            Return mZip_code
        End Get
        Set(ByVal value As String)
            mZip_code = value
        End Set
    End Property

    Public Property Curr_licence() As String
        Get
            Return mCurr_licence
        End Get
        Set(ByVal value As String)
            mCurr_licence = value
        End Set
    End Property

    Public Property Account2() As String
        Get
            Return mAccount2
        End Get
        Set(ByVal value As String)
            mAccount2 = value
        End Set
    End Property

    Public Property Base_amount() As String
        Get
            Return mBase_amount
        End Get
        Set(ByVal value As String)
            mBase_amount = value
        End Set
    End Property

    Public Property Base_curr() As String
        Get
            Return mBase_curr
        End Get
        Set(ByVal value As String)
            mBase_curr = value
        End Set
    End Property

    Public Property Pay_temp_id() As String
        Get
            Return mPay_temp_id
        End Get
        Set(ByVal value As String)
            mPay_temp_id = value
        End Set
    End Property

    Public Property Allocation_key() As String
        Get
            Return mAllocation_key
        End Get
        Set(ByVal value As String)
            mAllocation_key = value
        End Set
    End Property

    Public Property Period_no() As String
        Get
            Return mPeriod_no
        End Get
        Set(ByVal value As String)
            mPeriod_no = value
        End Set
    End Property

    Public Property Clearing_code() As String
        Get
            Return mClearing_code
        End Get
        Set(ByVal value As String)
            mClearing_code = value
        End Set
    End Property

    Public Property Swift() As String
        Get
            Return mSwift
        End Get
        Set(ByVal value As String)
            mSwift = value
        End Set
    End Property

    Public Property Arrive_id() As String
        Get
            Return mArrive_id
        End Get
        Set(ByVal value As String)
            mArrive_id = value
        End Set
    End Property

    Public Property Bank_acc_type() As String
        Get
            Return mBank_acc_type
        End Get
        Set(ByVal value As String)
            mBank_acc_type = value
        End Set
    End Property

    Private Shared Function makeNpsaFromRow( _
            ByVal r As IDataReader _
        ) As Npsa
        Return New Npsa( _
                clsNYS.notString(r.Item("batch_id")), _
                clsNYS.notString(r.Item("interface")), _
                clsNYS.notString(r.Item("voucher_type")), _
                clsNYS.notString(r.Item("trans_type")), _
                clsNYS.notString(r.Item("client")), _
                clsNYS.notString(r.Item("account")), _
                clsNYS.notString(r.Item("dim_1")), _
                clsNYS.notString(r.Item("dim_2")), _
                clsNYS.notString(r.Item("dim_3")), _
                clsNYS.notString(r.Item("dim_4")), _
                clsNYS.notString(r.Item("dim_5")), _
                clsNYS.notString(r.Item("dim_6")), _
                clsNYS.notString(r.Item("dim_7")), _
                clsNYS.notString(r.Item("tax_code")), _
                clsNYS.notString(r.Item("tax_system")), _
                clsNYS.notString(r.Item("currency")), _
                clsNYS.notString(r.Item("dc_flag")), _
                clsNYS.notString(r.Item("cur_amount")), _
                clsNYS.notString(r.Item("amount")), _
                clsNYS.notString(r.Item("number_1")), _
                clsNYS.notString(r.Item("value_1")), _
                clsNYS.notString(r.Item("value_2")), _
                clsNYS.notString(r.Item("value_3")), _
                clsNYS.notString(r.Item("descript")), _
                clsNYS.notString(r.Item("trans_date")), _
                clsNYS.notString(r.Item("voucher_date")), _
                clsNYS.notString(r.Item("voucher_no")), _
                clsNYS.notString(r.Item("period")), _
                clsNYS.notString(r.Item("tax_id")), _
                clsNYS.notString(r.Item("ext_inv_ref")), _
                clsNYS.notString(r.Item("ext_ref")), _
                clsNYS.notString(r.Item("due_date")), _
                clsNYS.notString(r.Item("disc_date")), _
                clsNYS.notString(r.Item("discount")), _
                clsNYS.notString(r.Item("commitment")), _
                clsNYS.notString(r.Item("order_id")), _
                clsNYS.notString(r.Item("kid")), _
                clsNYS.notString(r.Item("pay_transfer")), _
                clsNYS.notString(r.Item("status")), _
                clsNYS.notString(r.Item("apar_type")), _
                clsNYS.notString(r.Item("apar_id")), _
                clsNYS.notString(r.Item("pay_flag")), _
                clsNYS.notString(r.Item("voucher_ref")), _
                clsNYS.notString(r.Item("sequence_ref")), _
                clsNYS.notString(r.Item("intrule_id")), _
                clsNYS.notString(r.Item("factor_short")), _
                clsNYS.notString(r.Item("responsible")), _
                clsNYS.notString(r.Item("apar_name")), _
                clsNYS.notString(r.Item("address")), _
                clsNYS.notString(r.Item("province")), _
                clsNYS.notString(r.Item("place")), _
                clsNYS.notString(r.Item("bank_account")), _
                clsNYS.notString(r.Item("pay_method")), _
                clsNYS.notString(r.Item("vat_reg_no")), _
                clsNYS.notString(r.Item("zip_code")), _
                clsNYS.notString(r.Item("curr_licence")), _
                clsNYS.notString(r.Item("account2")), _
                clsNYS.notString(r.Item("Base_amount")), _
                clsNYS.notString(r.Item("base_curr")), _
                clsNYS.notString(r.Item("pay_temp_id")), _
                clsNYS.notString(r.Item("allocation_key")), _
                clsNYS.notString(r.Item("period_no")), _
                clsNYS.notString(r.Item("clearing_code")), _
                clsNYS.notString(r.Item("swift")), _
                clsNYS.notString(r.Item("arrive_id")), _
                clsNYS.notString(r.Item("bank_acc_type")))
    End Function

    Public Shared Function list(ByVal pstartdate As String, ByVal penddate As String) As List(Of Npsa)
        Using dbh As New SqlDatabaseHandle(getConnection)
            Dim ret As New List(Of Npsa)()
            Using r As IDataReader = dbh.callSP("FeederFile_NPSA", _
                                                "@startdate", pstartdate, _
                                                "@enddate", penddate)
                While r.Read()
                    ret.Add(makeNpsaFromRow(r))
                End While
            End Using
            Return ret
        End Using
    End Function

End Class
