-- =============================================
-- Seed Reference Data from Ref_Table_Extract.xlsx
-- =============================================

-- Only insert if not already seeded
IF NOT EXISTS (SELECT 1 FROM safety.ref_reference_type)
BEGIN

  -- ref_reference_type
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('2959c7b7-051a-4498-adf0-dc1897180bf9', 'documenttype', 'documenttype', 'attachment', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('1a603c02-a86e-4078-81e0-3f6126a5c575', 'environmentaltype', 'environmentaltype', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('88995ef3-b4ca-493a-8207-7d74ea37e845', 'equipmentdamage', 'equipmentdamage', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'incidenttype', 'incidenttype', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'injurydetail', 'injurydetail', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('820bd753-8df2-4896-b57c-b5167184507e', 'injurytype', 'injurytype', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'jobfactors', 'jobfactors', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'lifesupport', 'lifesupport', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('e1881b99-1b47-4d19-8f1f-14a28d9364f7', 'motorvehicleaccident', 'motorvehicleaccident', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'personnelfactors', 'personnelfactors', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('70626b79-ab21-4da3-95e9-7d21387c2d5f', 'policydeviation', 'policydeviation', 'incident_report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'rootcause', 'rootcause', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('24afa948-93d6-4e85-9fe5-0451c6017324', 'security', 'security', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('f2a536fe-ec72-4df9-af78-118b43e0404b', 'transportcompliance', 'transportcompliance', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('5ff8a921-0943-467f-9ef0-1b7edc93c224', 'unsafeacts', 'unsafeacts', 'investigation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_reference_type (id, code, name, applies_to, is_active, created_at, updated_at)
  VALUES ('fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'unsafeconditions', 'unsafeconditions', 'investigation', 1, GETUTCDATE(), GETUTCDATE());

  -- ref_incident_report_reference
  -- documenttype
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('25356709-36a8-40a2-8bbd-e133d1ddcb12', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'plan_permit', N'Plan Permit', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('688f8d62-78bf-405d-afb4-eac731c131f1', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'equipment_inspection', N'Equipment Inspection', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d82c3054-5c78-4068-b7c5-42a1ae925713', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'citation', N'Citation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('6b3e8aed-3464-4bec-a0ec-833e202ce6f1', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'strongcardflha', N'Strongcard/FLHA', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('763f4ebc-705a-4982-942f-0cd98fa9d345', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'policy_report', N'Policy Report', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9e0110e1-4bfa-47cf-bce5-9460691ed4b2', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'statement_of_all_involved', N'Statement of all Involved', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('a2204c33-319b-4f1c-aaa6-cd87ad74057d', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'daily_preshifttoolbox', N'Daily Pre-Shift/Toolbox', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ef2f7e71-6318-4530-af17-1ba7aabf4176', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'third_party_data', N'Third Party Data', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('39a81e77-0c2e-4a96-ad9a-6bf160e14e2d', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'photograph', N'Photograph', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e3137e42-2d64-4936-adae-258d56a9cacf', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'jha', N'JHA', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9818c936-9675-49f0-8603-5ff6e9c1eae9', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'training_records', N'Training Records', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('225b2079-5958-4d13-ae7f-20ecfbb1613e', '2959c7b7-051a-4498-adf0-dc1897180bf9', 'project_inspection', N'Project Inspection', 1, GETUTCDATE(), GETUTCDATE());

  -- environmentaltype
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9511bc87-4339-4930-b87c-f45ad947ba03', '1a603c02-a86e-4078-81e0-3f6126a5c575', 'emission', N'Emission', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1b2f1481-fe31-4495-86d7-a1b5c58e1dd4', '1a603c02-a86e-4078-81e0-3f6126a5c575', 'release', N'Release', 1, GETUTCDATE(), GETUTCDATE());

  -- equipmentdamage
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('890b8f1b-a6e4-4f4b-b5c4-607a330ac5ca', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'backing_up', N'Backing Up', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('735c01ff-b2e1-4030-8a36-251186fd26e7', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'fire', N'Fire', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('4d5d7ecd-649d-4e8a-a04a-d26fadc69a3a', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'malfunction', N'Malfunction', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('7817fc38-cd75-4e7b-8761-24a15d43638c', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'unreported_damage', N'Unreported Damage', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9a4ac7db-11c8-42f9-ac53-92af7b5474ab', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'windshield', N'Windshield', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('b4d99e6c-0ba6-4761-aefd-5420eb464970', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'explosion', N'Explosion', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('92aa6f73-c702-4869-a2cd-89969fac6664', '88995ef3-b4ca-493a-8207-7d74ea37e845', 'struck_by', N'Struck By', 1, GETUTCDATE(), GETUTCDATE());

  -- incidenttype
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9a68fbb8-8bb4-4dcf-8a80-28e3552ee0f2', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'transportation_compliance', N'Transportation Compliance', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('196db3a9-9a96-471d-bd5e-45161da8bd3e', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'injury', N'Injury', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('f386d531-a074-425b-88f4-3d30443e1ae1', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'environmental', N'Environmental', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('42fb58a7-24a1-45f3-a76e-a4fbfbffb64b', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'policy_deviation', N'Policy Deviation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('b756e19b-d244-46a1-882f-f9679c8b52f6', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'security', N'Security', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('90cdaf0f-9759-45a7-931a-9f130ba54aa2', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'equipment_damage', N'Equipment Damage', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e4025bc3-f6d6-452e-90a7-74b4545cab77', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'motor_vehicle_accident', N'Motor Vehicle Accident', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('aff9f6e6-bfd4-47fd-ada0-f13b5e2efb1b', '38763aac-5d5b-4fbc-bc01-ab1bf70d3f92', 'life_support', N'Life Support', 1, GETUTCDATE(), GETUTCDATE());

  -- injurydetail
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('54818cd2-ab6c-47b7-9d82-108f20c8d4f0', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'modified_work', N'Modified Work', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1ae166b6-193e-48f0-b9b5-75beec80dfbd', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'alleged', N'Alleged', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('f57ef15f-eb13-46a5-b887-35a05c7de652', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'lost_time', N'Lost Time', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('2a10beeb-27d7-4c65-885c-8427a1ecdce7', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'medical_aid', N'Medical Aid', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('7dc29d06-ef25-4eee-bfc8-5dadc368e08f', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'report_only', N'Report Only', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('c5027cf5-6044-486d-b943-60526f8f34e4', '284d8e6c-cc5d-4eb3-a8bb-602bf5ba7ff5', 'firstaid', N'First-Aid', 1, GETUTCDATE(), GETUTCDATE());

  -- injurytype
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e29f378b-dc1a-4641-b0f9-5606f683a1e9', '820bd753-8df2-4896-b57c-b5167184507e', 'fall_next_level', N'Fall Next Level', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('9cac758f-76aa-42b3-a706-cba2b4f8c5b0', '820bd753-8df2-4896-b57c-b5167184507e', 'caught_betweenunder', N'Caught Between/Under', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('35a564ea-9869-489f-8719-4468a2d410df', '820bd753-8df2-4896-b57c-b5167184507e', 'contact_with', N'Contact With', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('bb139b55-17ec-4b17-9c24-71b611e5ddd3', '820bd753-8df2-4896-b57c-b5167184507e', 'caught_in', N'Caught In', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('b5123b62-5d40-4ba7-911d-fd77a1749343', '820bd753-8df2-4896-b57c-b5167184507e', 'caught_on', N'Caught On', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('00e46a19-40a6-499c-ac73-7f2855c960bb', '820bd753-8df2-4896-b57c-b5167184507e', 'exposure', N'Exposure', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ba3d8950-be2f-46bd-9f16-8a1babbb8f77', '820bd753-8df2-4896-b57c-b5167184507e', 'overstress_ergonomics', N'Overstress Ergonomics', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ff65b928-dac4-44b0-a5bd-dea5c8e41a36', '820bd753-8df2-4896-b57c-b5167184507e', 'fall_same_level', N'Fall Same Level', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('cf698865-01ed-48ad-9221-d42927c6b385', '820bd753-8df2-4896-b57c-b5167184507e', 'struck_by', N'Struck By', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d93641c6-20d3-4746-9dca-d7e4ca6b5712', '820bd753-8df2-4896-b57c-b5167184507e', 'struck_against', N'Struck Against', 1, GETUTCDATE(), GETUTCDATE());

  -- jobfactors
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('5193001a-d680-4ccc-b4d3-fa1a9e72b214', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'bulletin_reviews', N'Bulletin Reviews', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ac531e97-8f90-453c-bf9b-548b2826a0b8', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'equipment_inspections', N'Equipment Inspections', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('39bd96af-cdf7-45ed-8ef1-7fda026c8aa6', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'dvirpretrip', N'DVIR/Pre-Trip', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ab0a7632-a98c-49fa-a359-72b570817630', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'strong_card', N'Strong Card', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e65722fc-1f1f-48a7-980e-cba034deb0bf', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'policies', N'Policies', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('f47f4d19-d667-44c9-8012-37467c92bfae', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'site_inspection', N'Site Inspection', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('da73f9a0-b660-4a0d-9d8c-7501d8f8985a', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'master_jha', N'Master JHA', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('5c2f15be-a6d6-48f0-961e-b469cfd23a55', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'communicationreinforcement', N'Communication/Reinforcement', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('4d6f26b6-c986-4173-9c61-8e0a30babe91', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'standardcop', N'Standard/COP', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e164303d-39b9-4d56-95b6-6b8a53bb291f', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'sop', N'SOP', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('594775ef-d560-4168-bf36-c5b4db8763d8', '484e3330-8fcc-463e-8a4e-dc495eb6c0d8', 'employee_engagements', N'Employee Engagements', 1, GETUTCDATE(), GETUTCDATE());

  -- lifesupport
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('0596422a-a937-4e72-8de8-93d87ce26df6', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'pigtail', N'Pigtail', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('89506543-1d3f-4ae1-a708-51f3253db3b8', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'helmet_malfunction', N'Helmet Malfunction', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('44e2fef6-a388-41b6-8508-cd18a30d7daa', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'harness', N'Harness', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ff065f7e-6212-401e-ae48-ae3ae9b09d19', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'trailercube', N'Trailer/Cube', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e805750c-a593-4479-903d-cc57bd977661', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'gas_detection', N'Gas Detection', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('af94c99c-2860-45d5-8a18-f23a65de3fe7', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'helmet_damage', N'Helmet Damage', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('961a636c-1553-4cf4-a4fd-56a46964b541', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'egress_cylinder', N'Egress Cylinder', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('31a38d8e-a395-48cc-b80d-36f705fe354d', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'module', N'Module', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1038ccdd-1e13-4a2a-9b76-21c0020f8196', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'egress_regulator', N'Egress Regulator', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e3fefb6e-de81-4608-a233-041f7e84391a', '0d7534e4-0b73-4a02-9e43-0ff7ac1f1348', 'umbilical', N'Umbilical', 1, GETUTCDATE(), GETUTCDATE());

  -- motorvehicleaccident
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('40fed18e-42cf-4be5-bb4c-a666be2e2e8e', 'e1881b99-1b47-4d19-8f1f-14a28d9364f7', 'third_party', N'Third Party', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('c00ec727-5e2a-4eaa-b24c-45e9753e7bcf', 'e1881b99-1b47-4d19-8f1f-14a28d9364f7', 'single_vehicle', N'Single Vehicle', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d6f2203d-faa5-430b-8ae8-7bda99991e6f', 'e1881b99-1b47-4d19-8f1f-14a28d9364f7', 'animal_strike', N'Animal Strike', 1, GETUTCDATE(), GETUTCDATE());

  -- personnelfactors
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('c265ae9c-e847-4028-9a01-8d186511e565', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'poor_brothers_sisters_keeper', N'Poor Brothers'' & Sisters'' Keeper', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('706e8554-a9ba-4236-be88-136d1a518e80', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'inadequate_policies', N'Inadequate Policies', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d967ca02-7922-4c04-8725-bd52f6a07f69', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'attitude', N'Attitude', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('0337a747-fdb8-4abb-9162-c0e127daba8f', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'missing_training', N'Missing Training', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('4bb4113a-9ddd-40bf-803c-51198693ace2', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'missing_policy', N'Missing Policy', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('5c467368-153a-49ae-a7f3-e144160785ab', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'inadequate_culture_building', N'Inadequate Culture Building', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ba9d97b8-5f0a-4349-90aa-9f5bd824648e', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'inadequate_training', N'Inadequate Training', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d9200f7d-0821-479d-832b-68a3057a92e7', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'inadequate_job_planning', N'Inadequate Job Planning', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e5423048-467e-436e-be27-86638a908ff4', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'rogue_employeeroot', N'Rogue EmployeeRoot', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('44194c2e-9018-43a5-8afe-58ab03c7c8d5', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'poor_communication', N'Poor Communication', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('901656d8-0d1d-468a-8a9d-cb650e333198', 'b11f27ed-4529-45c3-a4d6-8d5d5e192aba', 'inadequate_motivation', N'Inadequate Motivation', 1, GETUTCDATE(), GETUTCDATE());

  -- policydeviation
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('299c33d0-4968-44f5-863f-04fb830eb74e', '70626b79-ab21-4da3-95e9-7d21387c2d5f', 'shc_policy', N'SHC Policy', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('c1d44cbf-1273-44eb-8d9c-4659eb3dc078', '70626b79-ab21-4da3-95e9-7d21387c2d5f', 'client_policy', N'Client Policy', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('7949ac75-927b-4bcb-b253-45847203f928', '70626b79-ab21-4da3-95e9-7d21387c2d5f', 'lifesaving_rule_violation', N'Life-Saving Rule Violation', 1, GETUTCDATE(), GETUTCDATE());

  -- rootcause
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('3c0fcacf-8cd8-4b21-b310-fa5b9804a623', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'inadequate_job_planning', N'Inadequate Job Planning', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('63f40439-c4c6-4101-bcc1-3d418f448e6c', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'poor_brothers_sisters_keeper_attitude', N'Poor Brothers'' & Sisters'' Keeper Attitude', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('3994b14f-dd28-4881-adae-00e4019ea142', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'inadequate_culture_building', N'Inadequate Culture Building', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ab632db1-c805-4e82-93db-459b3fccfa43', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'rogue_employee', N'Rogue Employee', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('d02cd6a2-583e-4b81-a8f8-683c705ad558', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'poor_communication', N'Poor Communication', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('b8121c2a-1533-4c6d-b394-f1802e69c220', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'inadequate_motivation', N'Inadequate Motivation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('8c357eb0-b079-47b5-b5ce-ad26c3bcdbe3', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'missing_training', N'Missing Training', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('810f9078-d52a-46f5-b113-346b7a5528f7', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'inadequate_training', N'Inadequate Training', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('617ff521-ae18-47cd-a037-e73ee113e7f1', 'c16c7913-e3a5-4dfa-8cb1-5976094bba42', 'inadequate_policies', N'Inadequate Policies', 1, GETUTCDATE(), GETUTCDATE());

  -- security
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('2049d792-baf7-4f9f-9a3f-42584be0c7c1', '24afa948-93d6-4e85-9fe5-0451c6017324', 'break_in', N'Break In', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('2f7f4bec-6afc-4ea6-9d7e-fced7f1b2fec', '24afa948-93d6-4e85-9fe5-0451c6017324', 'theft', N'Theft', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('eaea4892-b56c-4988-b5c3-3e5b9452b338', '24afa948-93d6-4e85-9fe5-0451c6017324', 'vandelism', N'Vandelism', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('6fe0e7e3-27de-4be0-a16a-fa0e63df5899', '24afa948-93d6-4e85-9fe5-0451c6017324', 'arson', N'Arson', 1, GETUTCDATE(), GETUTCDATE());

  -- transportcompliance
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('44701f3f-1a53-4ad4-ba39-182bd6ccdc15', 'f2a536fe-ec72-4df9-af78-118b43e0404b', 'citations', N'Citations', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e67c84fa-51a2-4392-9a99-e300938371e2', 'f2a536fe-ec72-4df9-af78-118b43e0404b', 'public_driving_complaint', N'Public Driving Complaint', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('c596492a-9bd7-4674-a3b9-e1b219200109', 'f2a536fe-ec72-4df9-af78-118b43e0404b', 'roadside_inspection', N'Roadside Inspection', 1, GETUTCDATE(), GETUTCDATE());

  -- unsafeacts
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1220a65f-61b0-4f2b-af43-b2d4d8422136', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'bypassing_safety_controls', N'Bypassing Safety Controls', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('73f9895b-30e9-4277-8738-48a3c5b8767c', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'horseplay', N'Horseplay', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('46d5e3e2-9e07-4a85-bb85-2953c6b3be74', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'cse_violation', N'CSE Violation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('79e65b51-113c-4786-92b9-f0a4042a4227', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'energy_isolation', N'Energy Isolation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('692710b8-fe6b-421d-803f-89564ff568f2', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'failure_to_secure_equipment', N'Failure to Secure Equipment', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('6563eb39-c3d8-4596-9013-511a832b7b43', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'not_driving_to_road_conditions', N'Not Driving to Road Conditions', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('7c8c9e99-bd06-40ca-b62b-04f7449bd194', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'overconfident', N'Overconfident', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('fda92677-e573-4749-81dd-1a8efddf77be', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'failure_to_inspect', N'Failure to Inspect', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('a96a8980-0ae8-4e92-9b04-2130c90ed068', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'failure_to_identify_or_control_a_hazard', N'Failure to Identify or Control a Hazard', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('fd066d1b-a5ed-4ad3-93d8-7847b333c76c', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'fall_protection_violation', N'Fall Protection Violation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('86d32f08-5aec-4c75-b815-ccaefed77f6d', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'improper_use_of_toolsequipment', N'Improper use of tools/equipment', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1493729f-55e7-4324-bb43-4ff9d6493bcb', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'inattentive_to_hazards', N'Inattentive to Hazards', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('6ec76521-5414-41e8-9eb1-969aa2400df9', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'line_of_fire_positioning', N'Line of Fire Positioning', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('7c1035b9-b455-464b-8eb8-42db0db6b2d4', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'not_fit_for_duty', N'Not Fit for Duty', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('71c8b537-a445-4523-b165-aca2793634d6', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'trying_to_gain_or_save_time', N'Trying to Gain or Save Time', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('87ce94b2-9bfe-4f1c-aeec-b1294ac9b1b1', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'using_uninspected_equipment', N'Using Uninspected Equipment', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('432a82e3-c0be-44c0-a166-205f0429fe9e', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'workplace_violencesystemic', N'Workplace ViolenceSystemic', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e94ff48d-31b4-42b6-8065-1aa195c10b54', '5ff8a921-0943-467f-9ef0-1b7edc93c224', 'working_without_authority_permit_incorrect_permit', N'Working without Authority / Permit /Incorrect Permit', 1, GETUTCDATE(), GETUTCDATE());

  -- unsafeconditions
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('ef23aacc-51ee-4c89-a4d4-493568c10220', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'inadequate_procedure', N'Inadequate Procedure', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('a6981fb9-8c82-43c3-b09f-bf3413564f29', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'inadequate_training', N'Inadequate Training', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('99205789-c0eb-45d8-9cf0-dc61e77413ca', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'poor_housekeeping', N'Poor Housekeeping', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('1b0905a3-4dd1-424b-926d-b7f0e8715d1f', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'poor_ventilation', N'Poor Ventilation', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('3612fde6-948d-49bc-bd23-92f1ac583c0b', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'improper_storage', N'Improper Storage', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('2067b7ed-250f-49e6-8548-dc07a82bcd07', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'congestion', N'Congestion', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('829881b8-c0c4-43ef-add5-1c3e208424ab', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'inadequate_warning', N'Inadequate Warning', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('eef09e31-4bb6-46a6-a7ed-be778f3c4812', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'inadequate_guard', N'Inadequate Guard', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('98331a1b-72a0-417b-8f51-adadbcc876b3', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'fire_hazard', N'Fire Hazard', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('96831cbf-c673-47a7-9c64-051cd63084f7', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'poor_lighting', N'Poor Lighting', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('02da0859-ba27-4bcc-a91e-ce5b4f396e5d', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'road_conditions', N'Road Conditions', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('6dcbe0ae-b2d1-4a64-a7ad-a851f0531d78', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'weather', N'Weather', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('05daa918-8ac1-45c4-b187-7beda6a03a21', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'tripping_hazards', N'Tripping Hazards', 1, GETUTCDATE(), GETUTCDATE());
  INSERT INTO safety.ref_incident_report_reference (id, reference_type_id, code, name, is_active, created_at, updated_at)
  VALUES ('e9ae0f46-4dee-4237-9c99-8f8a3a71a529', 'fa68de37-c6a6-4539-bb3e-39bc45b8e2a8', 'condition_change', N'Condition Change', 1, GETUTCDATE(), GETUTCDATE());

END
GO