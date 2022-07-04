$(function(){
    // fix lá»—i qq gÃ¬ Ä‘Ã³ mÃ  nÃ³ ko chá»‹u show á»Ÿ láº§n nháº¥n Ä‘áº§u tiÃªn
    var is_first_click_toggle = true;
    $(".nav-link.dropdown-toggle").click(function(ev){
        if(is_first_click_toggle){
            is_first_click_toggle = false;
            setTimeout(()=>{
                $(".nav-link.dropdown-toggle").click();
            },0);
        }
    });
});