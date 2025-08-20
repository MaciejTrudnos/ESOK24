$(function() {
  var INDEX = 0; 
  var waiting = false;
  generate_message('Jak mogę Ci pomóc?', 'user'); 
 
  $("#chat-submit").click(async function(e) {
    e.preventDefault();

    let chatInputElem = $("#chat-input");
    var msg = chatInputElem.val(); 
    
    if(msg.trim() == ''){
      return false;
    }

    chatInputElem.prop('disabled', true);

    generate_message(msg, 'self');
    var buttons = [
        {
          name: 'Existing User',
          value: 'existing'
        },
        {
          name: 'New User',
          value: 'new'
        }
      ];

      await ask(msg);
      
      chatInputElem.prop('disabled', false);
      chatInputElem.focus();
  })

  function startThinking(){
    let text = "."
    generate_message(text, 'user');  
    
    waiting = true;
    while(waiting){
     text += "."
      setTimeout(function() {    
        $("#cm-msg-" + INDEX).text(text);
      }, 1000)
    }
    
  }

  async function ask(msg) {
    startThinking('self');

    var url = 'https://api.esok24.pl/answer';
    var question = '\"' + msg + '\"'

    await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'accept': '*/*'
      },
      body: question
    })
      .then(response => response.text())
      .then(data => {
        stopThinking();
        generate_message(data, 'user');  
      })
      .catch(error => {
        stopThinking();
        generate_message("Przeraszamy. Ale wygląda na to, że coś poszło nie tak", 'user');  
        console.error('Error:', error);
      });
  }

  function startThinking(type) {
    INDEX++;
    var str="";
    str += "<div id='cm-msg-"+INDEX+"' class=\"chat-msg "+type+"\">";
    str += "          <div class=\"spinner-grow\" role=\"status\">";
    str += "          <span class=\"sr-only\"> </span>";
    str += "          <\/div>";
    str += "        <\/div>";
    $(".chat-logs").append(str);
    $("#cm-msg-"+INDEX).hide().fadeIn(300);
    if(type == 'self'){
     $("#chat-input").val(''); 
    }    
    $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight}, 1000);    
  }  

  function stopThinking(){
    $("#cm-msg-"+INDEX).remove();
  }

  function generate_message(msg, type) {
    INDEX++;
    var str="";
    str += "<div id='cm-msg-"+INDEX+"' class=\"chat-msg "+type+"\">";
    str += "          <div class=\"cm-msg-text\">";
    str += msg;
    str += "          <\/div>";
    str += "        <\/div>";
    $(".chat-logs").append(str);
    $("#cm-msg-"+INDEX).hide().fadeIn(300);
    if(type == 'self'){
     $("#chat-input").val(''); 
    }    
    $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight}, 1000);    
  }  
  
  function generate_button_message(msg, buttons){    
    /* Buttons should be object array 
      [
        {
          name: 'Existing User',
          value: 'existing'
        },
        {
          name: 'New User',
          value: 'new'
        }
      ]
    */
    INDEX++;
    var btn_obj = buttons.map(function(button) {
       return  "              <li class=\"button\"><a href=\"javascript:;\" class=\"btn btn-primary chat-btn\" chat-value=\""+button.value+"\">"+button.name+"<\/a><\/li>";
    }).join('');
    var str="";
    str += "<div id='cm-msg-"+INDEX+"' class=\"chat-msg user\">";
    str += "          <div class=\"cm-msg-text\">";
    str += msg;
    str += "          <\/div>";
    str += "          <div class=\"cm-msg-button\">";
    str += "            <ul>";   
    str += btn_obj;
    str += "            <\/ul>";
    str += "          <\/div>";
    str += "        <\/div>";
    $(".chat-logs").append(str);
    $("#cm-msg-"+INDEX).hide().fadeIn(300);   
    $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight}, 1000);
    $("#chat-input").attr("disabled", true);
  }
  
  $(document).delegate(".chat-btn", "click", function() {
    var value = $(this).attr("chat-value");
    var name = $(this).html();
    $("#chat-input").attr("disabled", false);
    generate_message(name, 'self');
  })
  
  $("#chat-circle").click(function() {    
    $("#chat-circle").toggle('scale');
    $(".chat-box").toggle('scale');
  })
  
  $(".chat-box-toggle").click(function() {
    $("#chat-circle").toggle('scale');
    $(".chat-box").toggle('scale');
  })
  
})