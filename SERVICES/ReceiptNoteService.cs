using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IReceiptNoteService
    {
        ReceiptNote GetSingleById(int id);
        IEnumerable<ReceiptNote> GetAll();
        IEnumerable<ReceiptNote> GetAll(string searchString);
        IEnumerable<ReceiptNote> GetBySelectId(int id);
        void Insert(ReceiptNote entity);
        ReceiptNote Add(ReceiptNote entity);
        void Update(ReceiptNote entity);
        void Delete(int id);
        
    }
    public class ReceiptNoteService : IReceiptNoteService
    {
        private readonly IReceiptNoteRepository _receiptNoteRepository;

        public ReceiptNoteService(IReceiptNoteRepository receiptNoteRepository)
        {
            _receiptNoteRepository = receiptNoteRepository;
        }

        public ReceiptNote Add(ReceiptNote entity)
        {
            return _receiptNoteRepository.Add(entity);
        }

        public void Delete(int id)
        {
            _receiptNoteRepository.Delete(id);
        }

        public IEnumerable<ReceiptNote> GetAll()
        {
            return _receiptNoteRepository.GetAll();
        }

        public IEnumerable<ReceiptNote> GetAll(string searchString)
        {
            return _receiptNoteRepository.GetMulti(t => t.Code.Contains(searchString));
        }

        public IEnumerable<ReceiptNote> GetBySelectId(int id)
        {
            if (id == 1)
                return _receiptNoteRepository.GetMulti(t => t.Status == true);
            else if (id == 2)
                return _receiptNoteRepository.GetAll();
            return _receiptNoteRepository.GetMulti(t => t.Status == false);
        }

        public ReceiptNote GetSingleById(int id)
        {
            return _receiptNoteRepository.GetSingleById(id);
        }

        public void Insert(ReceiptNote entity)
        {
            _receiptNoteRepository.Insert(entity);
        }

        public void Update(ReceiptNote entity)
        {
            _receiptNoteRepository.Update(entity);
        }
    }
}
