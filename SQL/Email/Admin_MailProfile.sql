
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE Admin_MailProfile

AS
BEGIN
	

	SELECT * FROM msdb.dbo.sysmail_account;
	SELECT * FROM msdb.dbo.sysmail_profile;
	SELECT * FROM msdb.dbo.sysmail_profileaccount;
	
	EXEC msdb.dbo.sysmail_add_account_sp
	@account_name = 'CMS Mail Account',
	@description = 'Complaint Management System Mail',
	@email_address = 'support.complimatecms@gmail.com',
	@display_name = 'Complaint Management System',
	@mailserver_name = 'smtp.gmail.com',
	@port = 587,
	@enable_ssl = 1,
	@username = 'support.complimatecms@gmail.com',
	@password = 'vnferkawxvvxcuss';
	
	
	EXEC msdb.dbo.sysmail_add_profile_sp
	@profile_name = 'CMS Mail Profile',
	@description = 'Mail profile for Complaint Management System';
	
	EXEC msdb.dbo.sysmail_add_profileaccount_sp
	@profile_name = 'CMS Mail Profile',
	@account_name = 'CMS Mail Account',
	@sequence_number = 1;
	
	EXEC msdb.dbo.sysmail_add_principalprofile_sp
	@profile_name = 'CMS Mail Profile',
	@principal_name = 'public',
	@is_default = 1;
	
	EXEC msdb.dbo.sp_send_dbmail
	@profile_name = 'CMS Mail Profile',
	@recipients = 'piyumiparamee@gmail.com',
	@subject = 'Database Mail Test',
	@body = 'Database Mail is working successfully.';
	
	SELECT * 
	FROM msdb.dbo.sysmail_event_log
	ORDER BY log_date DESC;

END
GO
