(function(){
  function getById(id){ return document.getElementById(id); }
  function parseJson(s){ try { return JSON.parse(s); } catch(e){ return null; } }

  function hydrateHiddenToast(hfClientId){
    var hf = getById(hfClientId);
    if(!hf || !hf.value) return;
    var payload = parseJson(hf.value);
    if(payload && window.toast){ window.toast.show(payload.type, payload.message, payload.title); }
    hf.value = '';
  }
  window.toastHydrator = { run: hydrateHiddenToast };
})();
