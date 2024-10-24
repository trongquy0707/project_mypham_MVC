document.addEventListener('DOMContentLoaded', function() {
    var input = document.getElementById('selectedOption');
    var optionsList = document.querySelector('.custom-select .options');
    var options = optionsList.querySelectorAll('li');
  
    input.addEventListener('click', function() {
      optionsList.style.display = 'block';
    });
  
    options.forEach(function(option) {
      option.addEventListener('click', function() {
        input.value = this.getAttribute('data-value');
        optionsList.style.display = 'none';
      });
    });
  
    document.addEventListener('click', function(e) {
      if (!input.contains(e.target) && !optionsList.contains(e.target)) {
        optionsList.style.display = 'none';
      }
    });
  });