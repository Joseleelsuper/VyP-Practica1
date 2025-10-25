(function(){
  function isReload(){
    try {
      if (performance && performance.getEntriesByType) {
        var navs = performance.getEntriesByType('navigation');
        if (navs && navs.length) return navs[0].type === 'reload';
      }
      if (performance && performance.navigation && typeof performance.navigation.type === 'number'){
        // 1 == TYPE_RELOAD
        return performance.navigation.type === 1;
      }
    } catch(e) {}
    return false;
  }

  function clearFormInputs(){
    var form = document.querySelector('form');
    if(!form) return;
    var inputs = form.querySelectorAll('input[type="text"], input[type="email"], input[type="password"], input[type="number"], textarea');
    for (var i=0;i<inputs.length;i++) inputs[i].value = '';
  }

  function clearToastHiddenFields(){
    var hfs = document.querySelectorAll('input[type="hidden"][id*="hfToast"]');
    for (var i=0;i<hfs.length;i++) hfs[i].value = '';
  }

  function run(){
    if (isReload()){
      clearFormInputs();
      clearToastHiddenFields();
    }
  }

  if (document.readyState !== 'loading') run();
  else document.addEventListener('DOMContentLoaded', run);
})();
