(function(){
  function ensureContainer(){
    var el = document.getElementById('toast-container');
    if(!el){
      el = document.createElement('div');
      el.id = 'toast-container';
      el.className = 'toast-container';
      document.body.appendChild(el);
    }
    return el;
  }
  function escapeHtml(s){
    if(!s) return '';
    return s.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
  }
  function show(type, message, title){
    var container = ensureContainer();
    var toast = document.createElement('div');
    toast.className = 'toast ' + (type||'info');
    var h = '';
    if(title){ h += '<div class="title">'+escapeHtml(title)+'</div>'; }
    if(message){ h += '<div class="message">'+escapeHtml(message)+'</div>'; }
    toast.innerHTML = h;
    container.appendChild(toast);
    var hideMs = 3000;
    setTimeout(function(){
      toast.style.animation = 'toast-out .2s ease forwards';
      setTimeout(function(){
        if(toast && toast.parentNode){ toast.parentNode.removeChild(toast); }
      }, 180);
    }, hideMs);
  }
  window.toast = { show: show };
})();
