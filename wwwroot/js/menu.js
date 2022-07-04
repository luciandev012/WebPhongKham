$(document).ready(function() {
   const arrUrl = window.location.href.split('/');
   const menuActive = arrUrl[3];
   
   const sidebarMenu = $("#sidebar-menu li a");

   sidebarMenu.each(function(index, item) {
      const urlItem = $(item).attr('href');
      const arrUrlItem = urlItem.split('/');

      if(arrUrlItem[3] === menuActive) {
         $(item).closest('li').addClass('active');
      }
   });
});