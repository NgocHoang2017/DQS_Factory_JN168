using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace USBPortScanner
{
    class USBDeviceInfo
    {
         public USBDeviceInfo(ManagementObject device)
         {
             this.Name = device.GetPropertyValue("Name").ToString();
             this.Caption = device.GetPropertyValue("Caption").ToString();

             if (device.GetPropertyValue("ConfigManagerErrorCode") != null)
             {
                 this.ConfigManagerErrorCode = (ErrorCode)device.GetPropertyValue("ConfigManagerErrorCode");
             }
             else
             {
                 this.ConfigManagerErrorCode = ErrorCode.Device_is_not_present_not_working_properly_or_does_not_have_all_of_its_drivers_installed;
             }

             this.DeviceID = device.GetPropertyValue("DeviceID").ToString();
             this.SystemName = device.GetPropertyValue("SystemName").ToString();
             this.Status = device.GetPropertyValue("Status").ToString();
             
         }

         //Sets the variables above usable for our purpose in the Program.cs
         public string Name { get; private set; }
         public string Caption { get; private set; }
         public ErrorCode ConfigManagerErrorCode { get; private set; }
         public string DeviceID { get; private set; }
         public string SystemName { get; private set; }
         public string Status { get; private set; }
        
     }

     enum ErrorCode : uint
     {
         Device_is_working_properly = 0,
         Device_is_not_configured_correctly = 1,
         Windows_cannot_load_the_driver_for_this_device = 2,
         Driver_for_this_device_might_be_corrupted_or_the_system_may_be_low_on_memory_or_other_resources = 3,
         Device_is_not_working_properly_One_of_its_drivers_or_the_registry_might_be_corrupted = 4,
         Driver_for_the_device_requires_a_resource_that_Windows_cannot_manage = 5,
         Boot_configuration_for_the_device_conflicts_with_other_devices = 6,
         Cannot_filter = 7,
         Driver_loader_for_the_device_is_missing = 8,
         Device_is_not_working_properly_The_controlling_firmware_is_incorrectly_reporting_the_resources_for_the_device = 9,
         Device_cannot_start = 10,
         Device_failed = 11,
         Device_cannot_find_enough_free_resources_to_use = 12,
         Windows_cannot_verify_the_device_resources = 13,
         Device_cannot_work_properly_until_the_computer_is_restarted = 14,
         Device_is_not_working_properly_due_to_a_possible_reenumeration_problem = 15,
         Windows_cannot_identify_all_of_the_resources_that_the_device_uses = 16,
         Device_is_requesting_an_unknown_resource_type = 17,
         Device_drivers_must_be_reinstalled = 18,
         Failure_using_the_VxD_loader = 19,
         Registry_might_be_corrupted = 20,
         System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation_Windows_is_removing_the_device = 21,
         Device_is_disabled = 22,
         System_failure_If_changing_the_device_driver_is_ineffective_see_the_hardware_documentation = 23,
         Device_is_not_present_not_working_properly_or_does_not_have_all_of_its_drivers_installed = 24,
         Windows_is_still_setting_up_the_device = 25 | 26,
         Device_does_not_have_valid_log_configuration = 27,
         Device_drivers_are_not_installed = 28,
         Device_is_disabled_The_device_firmware_did_not_provide_the_required_resources = 29,
         Device_is_using_an_IRQ_resource_that_another_device_is_using = 30,
         Device_is_not_working_properly_Windows_cannot_load_the_required_device_drivers = 31,
        

        
    }
    
}

