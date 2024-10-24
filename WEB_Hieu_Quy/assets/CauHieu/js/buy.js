document.addEventListener('DOMContentLoaded', function () {
    let products = document.querySelectorAll('.buy .button');
  
    function handleBuyClick(quantityElement) {
      let currentQuantity = parseInt(quantityElement.textContent);
      quantityElement.textContent = currentQuantity + 1;
    }
  
    function handleReduceClick(quantityElement) {
      let currentQuantity = parseInt(quantityElement.textContent);
      if (currentQuantity > 0) {
        quantityElement.textContent = currentQuantity - 1;
      }
    }
  
    products.forEach(product => {
      let buyBtn = product.querySelector('.next');
      let reduceBtn = product.querySelector('.prev');
      let quantityElement = product.querySelector('#quantity');
  
      buyBtn.addEventListener('click', () => {
        handleBuyClick(quantityElement);
      });
  
      reduceBtn.addEventListener('click', () => {
        handleReduceClick(quantityElement);
      });
    });
  });
  
